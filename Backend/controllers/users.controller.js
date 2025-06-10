const User = require("../models/user.model.js");
const dbService = require("../services/dbservice.service.js");
const hashingService = require("../services/hashingservice.service.js");
const jwt = require('jsonwebtoken');

const verifyUserAsync = async(req, res) => {
    try
    {
        var token = req.body.token;
        var decodedToken;
        jwt.verify(token, 'secret', function(err, tokendata){
            if(err){
                return response.status(400).json({error: 'Unauthorized request'});
             }

              if(tokendata){
                decodedToken = tokendata;
              }
        })
    
        res.status(200).json({userId : decodedToken.userId});
    } catch (e){
        res.status(404).json({message : 'Token verification failed!:  ' + e.message});
    }
};

const registerAsync = async(req, res) => {
    try{
        if (req.body.userName === undefined || req.body.userName === null){
            res.status(400).json({message: 'The property req.body.userName was not defined or was null!'});
            return;
        }
  
        if (req.body.password === undefined || req.body.password === null){
            res.status(400).json({message: 'The property req.body.password was not defined or was null!'});
            return;
        }

        if (req.body.email === undefined || req.body.email === null){
            res.status(400).json({message: 'The property req.body.email was not defined or was null!'});
            return;
        }

        if (req.body.firstName === undefined){
            res.status(400).json({message: 'The property req.body.firstName was not defined!'});
            return;
        }

        if (req.body.lastName === undefined){
            res.status(400).json({message: 'The property req.body.lastName was not defined!'});
            return;
        }

        if (req.body.userName.length === 0){
            res.status(400).json({message: 'The username cannot be empty!'});
            return;
        }

        if (req.body.password.length === 0){
            res.status(400).json({message: 'The password cannot be empty!'});
            return;
        }

        if (req.body.email.length === 0){
            res.status(400).json({message: 'The email cannot be empty!'});
            return;
        }

        let passwordTask = hashingService.hash(req.body.password);
        let emailTask = dbService.doesEmailExist(req.body.email);
        let usernameTask = dbService.doesUsernameExist(req.body.userName);

        let usernameAlreadyExists = await usernameTask;

        if (usernameAlreadyExists){
            res.status(400).json({message: 'The username already exists!'});
            return;
        }

        let emailAlreadyExists = await emailTask;

        if (emailAlreadyExists){
            res.status(400).json({message: 'The email already exists!'});
            return;
        }

        passwordTask.then((hashed) => {
            let user = new User(0, req.body.firstName, req.body.lastName, req.body.userName, req.body.email, hashed);
            dbService.insertUser(user).then((resultId) => {
                res.status(201).json({message : `User with the ID ${resultId}  was successfully registered!`});
            },
            (error) => {
                res.status(500).json({message: 'The register operation failed: ' + error.message});
            });
        })
    }
    catch(e){
        res.status(500).json({message: 'The register operation failed: ' + e.message});
    }
}

const loginAsync = async(req, res) => {
    try{
        if (req.body.password === undefined || req.body.password === null){
            res.status(400).json({message: 'The property req.body.password was not defined or was null!'});
            return;
        }

        let user = null;
        let userNameTask = dbService.doesUsernameExist(req.body.userName);
        let emailTask = dbService.doesEmailExist(req.body.email);

        let usernameExists = await userNameTask;
        let emailExists = await emailTask;

        if (usernameExists){
            user = await dbService.getUserInformationByUsername(req.body.userName);
        }
        else if (emailExists){
            user = await dbService.getUserInformationByEmail(req.body.email);
        }
        else
        {
            res.status(400).json({message: 'Invalid login credentials detected!'});
            return;
        }


        hashingService.isValid(req.body.password, user.password).then((valid) => {
            if (valid){         
               var token = jwt.sign({userId: user.userId},'secret', {expiresIn : '3h'});
               res.status(200).json({token: token});
            }
            else{
                res.status(404).json({message : 'Login failed!:  ' + 'Wrong password'});
            }
        })
        .catch((error) => {
            res.status(404).json({message : 'The login operation failed: ' + error.message});
        })
    }
    catch(e){
        res.status(500).json({message: 'The login operation failed: ' + e.message});
    }
}

const updateAsync = async(req, res) => {
    try{
        if (req.params.userId === undefined || req.params.userId === null){
            res.status(400).json({message: 'The property req.params.userId was not defined or was null!'});
            return;
        }
     
        if (req.body.userId === undefined || req.body.userId === null){
           res.status(400).json({message: 'The property req.body.userId was not defined or was null!'});
           return;
        }
     
        if (req.body.userId != req.params.userId){
         res.status(400).json({message: 'The properties req.body.userId and req.params.userId must match!'});
         return;
        }
     
     
        if (req.body.userName === undefined || req.body.userName === null){
         res.status(400).json({message: 'The property req.body.userName was not defined or was null!'});
         return;
        }
     
         if (req.body.email === undefined || req.body.email === null){
             res.status(400).json({message: 'The property req.body.email was not defined or was null!'});
             return;
         }
     
         if (req.body.firstName === undefined){
             res.status(400).json({message: 'The property req.body.firstName was not defined!'});
             return;
         }
     
         if (req.body.lastName === undefined){
             res.status(400).json({message: 'The property req.body.lastName was not defined!'});
             return;
         }
     
         if (req.body.userName.length === 0){
             res.status(400).json({message: 'The username cannot be empty!'});
             return;
         }
     
         if (req.body.email.length === 0){
             res.status(400).json({message: 'The email cannot be empty!'});
             return;
         }
     
         let uid = Number(req.body.userId);
         let emailExistsTask = dbService.doesEmailExistExcept(req.body.email, uid);
         let userExistsTask = dbService.doesUsernameExistExcept(req.body.userName, uid);
         let userNameExists = await userExistsTask;
     
         if (userNameExists){
             res.status(400).json({message: 'Other user by the given username already exists!'});
             return;  
         }
     
         let emailExists = await emailExistsTask;
     
         if (emailExists){
             res.status(400).json({message: 'Other user by the given email already exists!'});
             return;  
         }
     
         let user = new User(uid, req.body.firstName, req.body.lastName,
             req.body.userName, req.body.email, req.body.password);
     
         dbService.updateUser(user, uid).then((userId) => {
             if (userId === uid){
                 res.status(200).json({message: `The user with the ID ${req.params.userId} was successfully updated!`});
                 return;
             }

             res.status(500).json({message: 'The update operation failed: '+ err.message});
         }, (err) => {
            res.status(500).json({message: 'The update operation failed: '+ err.message});
         })
    }
    catch(e){
        res.status(500).json({message: 'The update operation failed: '+ e.message});
    }
}

const deleteAsync = async(req, res) => {
    try{
        if (req.params.userId === undefined || req.params.userId === null || req.params.userId === ''){
            res.status(400).json({message: 'The property req.params.userId was not defined or was null!'});
            return;
        }

        var id = Number(req.params.userId);
        var deletedId = await dbService.deleteUser(id);
        res.status(200).json({message: `The user with ID ${deletedId} was successfully deleted!`});
    }
    catch(e){
        res.status(500).json({message: 'The delete user operation failed: ' + e.message});
    }
}

const updatePasswordAsync = async(req, res) => {
    try{
        if (req.params.userId === undefined || req.params.userId === null || req.params.userId === ''){
            res.status(400).json({message: 'The property req.params.userId was not defined or was null!'});
            return;
        }

        if (req.params.userId === undefined || req.params.userId === null){
            res.status(400).json({message: 'The property req.params.userId was not defined or was null!'});
            return;
        }
     
        if (req.body.userId === undefined || req.body.userId === null){
           res.status(400).json({message: 'The property req.body.userId was not defined or was null!'});
           return;
        }

        if (req.body.password === undefined || req.body.password === null){
           res.status(400).json({message: 'The property req.body.password was not defined or was null!'});
           return;
        }

        if (req.body.password.length === 0){
            res.status(400).json({message: 'The password cannot be empty!'});
            return;
        }

        let uid = Number(req.params.userId);
        let password = await hashingService.hash(req.body.password);
        
        dbService.updatePassword(uid, password).then((userId) => {
            if (userId === uid){
                res.status(200).json({message: `The password for the user with th ID ${req.params.userId} was successfully updated!`});
                return;
            }

            res.status(500).json({message: 'The update password operation failed: '+ e.message});
        }, (err) => {
            res.status(500).json({message: 'The update password operation failed: '+ err.message});  
        })
    }
    catch(e){
        res.status(500).json({message: 'The update passwords operation failed: ' + e.message});
    }
}

const getUserInformationAsync = async(req, res) => {
    try{
        if (req.params.userId === undefined || req.params.userId === null || req.params.userId === ''){
            res.status(400).json({message: 'The property req.params.userId was not defined or was null!'});
            return;
        }
        
        var id = Number(req.params.userId);
        var user = await dbService.getUserInformation(id);
        res.status(200).json(user);
    }
    catch(e){
        res.status(500).json({message: 'The get user information operation failed: ' + e.message});
    }
}

module.exports = {
    registerAsync,
    loginAsync,
    updateAsync,
    deleteAsync,
    verifyUserAsync,
    updatePasswordAsync,
    getUserInformationAsync
};