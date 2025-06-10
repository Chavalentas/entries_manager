const Entry = require("../models/entry.model.js");
const dbService = require("../services/dbservice.service.js");

const createAsync = async(req, res) => {
    try{
        if (req.body.entryText === undefined){
            res.status(400).json({message: 'The property req.body.entryText was not defined!'});
            return;
        }

        if (req.body.entryTitle === undefined){
            res.status(400).json({message: 'The property req.body.entryTitle was not defined!'});
            return;
        }
  
        if (req.body.userId === undefined || req.body.userId === null){
            res.status(400).json({message: 'The property req.body.userId was not defined or was null!'});
            return;
        }

        if (req.body.entryDate === undefined || req.body.entryDate === null){
            res.status(400).json({message: 'The property req.body.entryDate was not defined or was null!'});
            return;
        }

        let uid = Number(req.body.userId);
        let userTask = dbService.doesUserIdExist(uid);

        let userExists = await userTask;

        if (!userExists){
            res.status(400).json({message: 'The user does not exist!'});
            return;
        }

        let entry = new Entry(0, req.body.entryText, req.body.entryTitle, req.body.entryDate, uid);
        dbService.createNewEntry(entry).then((resultId) => {
            res.status(201).json({message : `Entry with the ID ${resultId}  was successfully created!`});
        },
        (error) => {
            res.status(500).json({message: 'The create entry operation failed: ' + error.message});
        });
    }
    catch(e){
        res.status(500).json({message: 'The create entry operation failed: '+ e.message});
    }
}

const updateAsync = async(req, res) => {
    try{
        if (req.params.entryId === undefined || req.params.entryId === null){
            res.status(400).json({message: 'The property req.params.entryId was not defined or was null!'});
            return;
        }

        if (req.body.entryId === undefined || req.body.entryId === null){
            res.status(400).json({message: 'The property req.body.entryId was not defined or was null!'});
            return;
         }
     
        if (req.body.userId === undefined || req.body.userId === null){
           res.status(400).json({message: 'The property req.body.userId was not defined or was null!'});
           return;
        }

        if (req.body.entryId != req.params.entryId){
         res.status(400).json({message: 'The properties req.body.entryId and req.params.entryId must match!'});
         return;
        }
     
        if (req.body.entryText === undefined || req.body.entryText === null){
         res.status(400).json({message: 'The property req.body.entryText was not defined or was null!'});
         return;
        }

        if (req.body.entryTitle === undefined || req.body.entryTitle === null){
            res.status(400).json({message: 'The property req.body.entryTitle was not defined or was null!'});
            return;
        }
     
         let uid = Number(req.body.userId);
         let eid = Number(req.params.entryId)
         let userExistsTask = dbService.doesUserIdExist(uid);
         let userExists = await userExistsTask;
     
         if (!userExists){
             res.status(400).json({message: `The user with the ID ${uid} does not exist!`});
             return;  
         }
     
         let entry = new Entry(eid, req.body.entryText, req.body.entryTitle, "", uid);
     
         dbService.updateEntry(eid, entry).then((entryId) => {
             if (entryId === eid){
                 res.status(200).json({message: `The entry with the ID ${entryId} was successfully updated!`});
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
        if (req.params.entryId === undefined || req.params.entryId === null || req.params.entryId === ''){
            res.status(400).json({message: 'The property req.params.entryId was not defined or was null!'});
            return;
        }

        var id = Number(req.params.entryId);
        var deletedId = await dbService.deleteEntry(id);
        res.status(200).json({message: `The entry with ID ${deletedId} was successfully deleted!`});
    }
    catch(e){
        res.status(500).json({message: 'The delete entry operation failed: ' + e.message});
    }
}

const getAsync = async(req, res) => {
    try{
        if (req.params.userId === undefined || req.params.userId === null || req.params.userId === ''){
            res.status(400).json({message: 'The property req.params.userId was not defined or was null!'});
            return;
        }

        var id = Number(req.params.userId);
        dbService.getEntries(id).then((entries) => {
            res.status(200).json(entries);
        }, (err) => {
            res.status(500).json({message: 'The get entries operation failed: ' + err.message}); 
        })
    }
    catch(e){
        res.status(500).json({message: 'The get entries operation failed: ' + e.message});
    }
}

module.exports = {
    getAsync,
    createAsync,
    updateAsync,
    deleteAsync,
};