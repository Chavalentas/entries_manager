const dbConfig = require('../config/database.config.js');
const loggerService = require('./loggerservice.service.js');
var Connection = require('tedious').Connection;  
var Request = require('tedious').Request;  
var TYPES = require('tedious').TYPES; 
const User = require("../models/user.model.js");
const Entry = require('../models/entry.model.js');


const initialize = async() => {
    return new Promise(function(resolve, reject){
        connect().then((conn) => {
            conn.close();
            resolve();
        }, (err) => {
            reject(err);
        })
    });
}

const connect = async() => {
    return new Promise(function(resolve, reject){
        let config = getConfig();
        let connection = new Connection(config);
        connection.connect(function(err){
            if (err !== undefined){
                loggerService.logError("Connection to the database failed!");
                reject(err);
                return;
            }
    
            loggerService.logInfo("Connection to the database was successful!");
            resolve(connection);
        });
    })
}

const updateUser = async(user, userId) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            if (!(user instanceof User)){
                throw new Error("Invalid data type detected!")
            }

            if (typeof userId !== "number"){
                throw new Error("Invalid data type detected!")
            }

            if (userId !== user.userId){
                throw new Error("The IDs do not match!")
            }

            let query = "UPDATE [User] SET FirstName = @FirstName, LastName = @LastName, UserName = @UserName, Email = @Email " +
            "WHERE UserId = @UserId";
            var request = new Request(query, function(err) {  
                if (err) {  
                   reject(err);
                   return;  
               }});  

               request.addParameter('FirstName', TYPES.VarChar, user.firstName);  
               request.addParameter('LastName', TYPES.VarChar , user.lastName);  
               request.addParameter('UserName', TYPES.VarChar, user.userName);  
               request.addParameter('Email', TYPES.VarChar, user.email); 
               request.addParameter("UserId", TYPES.BigInt, userId);

               request.on("requestCompleted", function (rowCount, more) {
                   connection.close();
                   resolve(userId);
               });

               connection.execSql(request);  
        }, (err) => {
            reject(err);
        });
    }, (err) => {
        reject(err);
    })
}

const updatePassword = async(userId, value) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            if (typeof userId !== "number"){
                throw new Error("Invalid data type detected!")
            }
        
            if (typeof value !== "string"){
                throw new Error("Invalid data type detected!")
            }
        
            let query = `UPDATE [User] SET Password = @Password WHERE UserId = @UserId`;
            var request = new Request(query, function(err) {  
                if (err) {  
                   reject(err);
                   return;  
               }});  

               request.addParameter("Password", TYPES.VarChar, value);
               request.addParameter("UserId", TYPES.BigInt, userId);

               request.on("requestCompleted", function (rowCount, more) {
                   connection.close();
                   resolve(userId);
               });

               connection.execSql(request);  
        }, (err) => {
            reject(err);
        });
    }, (err) => {
        reject(err);
    })
}

const deleteUser = async(userId) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            if (typeof userId !== "number"){
                throw new Error("Invalid data type detected!")
            }

            let query = "DELETE FROM [User] WHERE UserId = @UserId";

            var request = new Request(query, function(err) {  
                if (err) {  
                   reject(err);
                   return;  
               }});  

               request.addParameter("UserId", TYPES.BigInt, userId);

               request.on("requestCompleted", function (rowCount, more) {
                   connection.close();
                   resolve(userId);
               });

               connection.execSql(request);  
        }, (err) => {
            reject(err);
        });
    }, (err) => {
        reject(err);
    })
}

const insertUser = async(user) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            if (!(user instanceof User)){
                throw new Error("Invalid data type detected!")
            }

            let query = "insert into [User] (FirstName, LastName, UserName, Password, Email)" +
            "values (@FirstName, @LastName, @UserName, @Password, @Email); select @@identity";
            var request = new Request(query, function(err) {  
                if (err) {  
                   reject(err);
                   return;  
               }});  

               let resultId = 0;
               request.addParameter('FirstName', TYPES.VarChar, user.firstName);  
               request.addParameter('LastName', TYPES.VarChar , user.lastName);  
               request.addParameter('UserName', TYPES.VarChar, user.userName);  
               request.addParameter('Password', TYPES.VarChar, user.password);  
               request.addParameter('Email', TYPES.VarChar, user.email); 

               request.on('row', function(columns) {  
                resultId = columns[0].value;
               });

               request.on("requestCompleted", function (rowCount, more) {
                   connection.close();
                   resolve(resultId);
               });

               connection.execSql(request);  
        }, (err) => {
            reject(err);
        });
    }, (err) => {
        reject(err);
    })
}

const doesUserIdExist = async(userId) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            let query = "SELECT dbo.DoesUserIdExist(@UserId)";
            let result = 0;
            var request = new Request(query, function(err) {  
                if (err) {  
                   reject(err);
                   return;  
               }});  
 
               request.addParameter('UserId', TYPES.BigInt, userId);  

               request.on('row', function(columns) {  
                result = columns[0].value;
               });

               request.on("requestCompleted", function (rowCount, more) {
                   connection.close();
                   resolve(result);
               });

               connection.execSql(request);  
        }, (err) => {
            reject(err);
        });
    }, (err) => {
        reject(err);
    })
}

const doesUsernameExist = async(userName) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            let query = "SELECT dbo.DoesUsernameExist(@Username)";
            let result = 0;
            var request = new Request(query, function(err) {  
                if (err) {  
                   reject(err);
                   return;  
               }});  
 
               request.addParameter('Username', TYPES.VarChar, userName);  

               request.on('row', function(columns) {  
                result = columns[0].value;
               });

               request.on("requestCompleted", function (rowCount, more) {
                   connection.close();
                   resolve(result);
               });

               connection.execSql(request);  
        }, (err) => {
            reject(err);
        });
    }, (err) => {
        reject(err);
    })
}

const doesUsernameExistExcept = async(userName, userId) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            if (!(typeof userId === 'number')){
                throw new Error("Invalid data type detected!");
            }

            let query = "SELECT dbo.DoesUsernameExistExceptId(@UserName, @UserId)";
            let result = 0;
            var request = new Request(query, function(err) {  
                if (err) {  
                   reject(err);
                   return;  
               }});  
 
               request.addParameter('UserName', TYPES.VarChar, userName);  
               request.addParameter('UserId', TYPES.BigInt, userId);  

               request.on('row', function(columns) {  
                result = columns[0].value;
               });

               request.on("requestCompleted", function (rowCount, more) {
                   connection.close();
                   resolve(result);
               });

               connection.execSql(request);  
        }, (err) => {
            reject(err);
        });
    }, (err) => {
        reject(err);
    })
}

const doesEmailExist = async(email) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            let query = "SELECT dbo.DoesEmailExist(@Email)";
            let result = 0;
            var request = new Request(query, function(err) {  
                if (err) {  
                   reject(err);
                   return;  
               }});  
 
               request.addParameter('Email', TYPES.VarChar, email);  

               request.on('row', function(columns) {  
                result = columns[0].value;
               });

               request.on("requestCompleted", function (rowCount, more) {
                   connection.close();
                   resolve(result);
               });

               connection.execSql(request);  
        }, (err) => {
            reject(err);
        });
    }, (err) => {
        reject(err);
    })
}

const doesEmailExistExcept = async(email, userId) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            if (!(typeof userId === 'number')){
                throw new Error("Invalid data type detected!");
            }

            let query = "SELECT dbo.DoesEmailExistExceptId(@Email, @UserId)";
            let result = 0;
            var request = new Request(query, function(err) {  
                if (err) {  
                   reject(err);
                   return;  
               }});  
 
               request.addParameter('Email', TYPES.VarChar, email);  
               request.addParameter('UserId', TYPES.BigInt, userId);  

               request.on('row', function(columns) {  
                result = columns[0].value;
               });

               request.on("requestCompleted", function (rowCount, more) {
                   connection.close();
                   resolve(result);
               });

               connection.execSql(request);  
        }, (err) => {
            reject(err);
        });
    }, (err) => {
        reject(err);
    })
}

const getUserInformation = async(userId) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            if (typeof userId !== "number"){
                throw new Error("Invalid data type detected!")
            }
    
            var request = new Request(`SELECT * FROM [User] where UserId = ${userId};`, function(err) {  
                if (err) {  
                    reject(err);
                    return; 
                }});  

                
                let uid = 0;
                let userName = null;
                let firstName = null;
                let lastName = null;
                let email = null;
                let password = null;

                request.on('row', function(columns) {  
                    columns.forEach(function(column) {  
                        if (column.metadata.colName === 'UserId'){
                            uid = column.value;
                        }

                        if (column.metadata.colName === 'UserName'){
                            userName = column.value;
                        }

                        if (column.metadata.colName === 'FirstName'){
                            firstName = column.value;
                        }
    
                        if (column.metadata.colName === 'LastName'){
                            lastName = column.value;
                        }

                        if (column.metadata.colName === 'Email'){
                            email = column.value;
                        }

                        if (column.metadata.colName === 'Password'){
                            password = column.value;
                        }
                    });  
                });  
                
                request.on("requestCompleted", function (rowCount, more) {
                    let user = new User(uid, firstName, lastName, userName, email, password);
                    connection.close();
                    resolve(user);
                });
    
                connection.execSql(request);  
        }, (err) => {
            reject(err);
        })
    }, (err) => {
        reject(err);
    })
}

const getUserInformationByUsername = async(username) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            if (typeof username !== "string"){
                throw new Error("Invalid data type detected!")
            }
    
            var request = new Request(`SELECT * FROM [User] where UserName = @UserName;`, function(err) {  
                if (err) {  
                    reject(err);
                    return; 
                }});  

            request.addParameter('UserName', TYPES.VarChar, username);  

                
                let uid = 0;
                let userName = null;
                let firstName = null;
                let lastName = null;
                let email = null;
                let password = null;

                request.on('row', function(columns) {  
                    columns.forEach(function(column) {  
                        if (column.metadata.colName === 'UserId'){
                            uid = column.value;
                        }

                        if (column.metadata.colName === 'UserName'){
                            userName = column.value;
                        }

                        if (column.metadata.colName === 'FirstName'){
                            firstName = column.value;
                        }
    
                        if (column.metadata.colName === 'LastName'){
                            lastName = column.value;
                        }

                        if (column.metadata.colName === 'Email'){
                            email = column.value;
                        }

                        if (column.metadata.colName === 'Password'){
                            password = column.value;
                        }
                    });  
                });  
                
                request.on("requestCompleted", function (rowCount, more) {
                    let user = new User(uid, firstName, lastName, userName, email, password);
                    connection.close();
                    resolve(user);
                });
    
                connection.execSql(request);  
        }, (err) => {
            reject(err);
        })
    }, (err) => {
        reject(err);
    })
}

const getUserInformationByEmail = async(email) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            if (typeof email !== "string"){
                throw new Error("Invalid data type detected!")
            }
    
            var request = new Request(`SELECT * FROM [User] where Email = @Email;`, function(err) {  
                if (err) {  
                    reject(err);
                    return; 
                }});  

            request.addParameter('Email', TYPES.VarChar, email);  

                
                let uid = 0;
                let userName = null;
                let firstName = null;
                let lastName = null;
                let em = null;
                let password = null;

                request.on('row', function(columns) {  
                    columns.forEach(function(column) {  
                        if (column.metadata.colName === 'UserId'){
                            uid = column.value;
                        }

                        if (column.metadata.colName === 'UserName'){
                            userName = column.value;
                        }

                        if (column.metadata.colName === 'FirstName'){
                            firstName = column.value;
                        }
    
                        if (column.metadata.colName === 'LastName'){
                            lastName = column.value;
                        }

                        if (column.metadata.colName === 'Email'){
                            em = column.value;
                        }

                        if (column.metadata.colName === 'Password'){
                            password = column.value;
                        }
                    });  
                });  
                
                request.on("requestCompleted", function (rowCount, more) {
                    let user = new User(uid, firstName, lastName, userName, em, password);
                    connection.close();
                    resolve(user);
                });
    
                connection.execSql(request);  
        }, (err) => {
            reject(err);
        })
    }, (err) => {
        reject(err);
    })
}

const getEntries = async(userId) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            if (typeof userId !== "number"){
                throw new Error("Invalid data type detected!")
            }
    
            var request = new Request(`SELECT * FROM [Entry] where UserId = ${userId};`, function(err) {  
                if (err) {  
                    reject(err);
                    return; 
                }});  

                
                let result = []

                request.on('row', function(columns) {  
                    let entryId = 0;
                    let entryText = null;
                    let entryDate = null;
                    let entryTitle = null;
                    let uid = 0;

                    columns.forEach(function(column) {  
                        if (column.metadata.colName === 'UserId'){
                            uid = column.value;
                        }

                        if (column.metadata.colName === 'EntryId'){
                            entryId = column.value;
                        }

                        if (column.metadata.colName === 'EntryText'){
                            entryText = column.value;
                        }
    
                        if (column.metadata.colName === 'EntryDate'){
                            entryDate = column.value;
                        }

                        if (column.metadata.colName === 'EntryTitle'){
                            entryTitle = column.value;
                        }
                    });  

                    let toAdd = new Entry(entryId, entryText, entryTitle, entryDate, uid);
                    result.push(toAdd);
                });  
                
                request.on("requestCompleted", function (rowCount, more) {
                    connection.close();
                    resolve(result);
                });
    
                connection.execSql(request);  
        }, (err) => {
            reject(err);
        })
    }, (err) => {
        reject(err);
    })
}

const createNewEntry = async(entry) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            if (!(entry instanceof Entry)){
                throw new Error("Invalid data type detected!")
            }

            let query = "EXEC CreateNewEntry @UserId, @EntryDate, @EntryTitle, @EntryText;select @@identity";
            var request = new Request(query, function(err) {  
                if (err) {  
                   reject(err);
                   return;  
               }});  

               let resultId = 0;
               request.addParameter('EntryText', TYPES.Text, entry.entryText);  
               request.addParameter('EntryDate', TYPES.VarChar, entry.entryDate);  
               request.addParameter('UserId', TYPES.BigInt, entry.userId);  
               request.addParameter('EntryTitle', TYPES.VarChar, entry.entryTitle);

               request.on('row', function(columns) {  
                resultId = columns[0].value;
               });

               request.on("requestCompleted", function (rowCount, more) {
                   connection.close();
                   resolve(resultId);
               });

               connection.execSql(request);  
        }, (err) => {
            reject(err);
        });
    }, (err) => {
        reject(err);
    })
}

const updateEntry = async(entryId, entry) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            if (!(entry instanceof Entry)){
                throw new Error("Invalid data type detected!")
            }

            if (typeof entryId !== "number"){
                throw new Error("Invalid data type detected!")
            }

            if (entryId !== entry.entryId){
                throw new Error("The IDs do not match!")
            }

            let query = "UPDATE [Entry] SET EntryText = @EntryText, EntryTitle = @EntryTitle WHERE EntryId = @EntryId";
            var request = new Request(query, function(err) {  
                if (err) {  
                   reject(err);
                   return;  
               }});  

               request.addParameter('EntryText', TYPES.Text, entry.entryText); 
               request.addParameter("EntryId", TYPES.BigInt, entryId);
               request.addParameter('EntryTitle', TYPES.VarChar, entry.entryTitle);

               request.on("requestCompleted", function (rowCount, more) {
                   connection.close();
                   resolve(entryId);
               });

               connection.execSql(request);  
        }, (err) => {
            reject(err);
        });
    }, (err) => {
        reject(err);
    })
}

const deleteEntry = async(entryId) => {
    return new Promise(function(resolve, reject){
        connect().then((connection) => {
            if (typeof entryId !== "number"){
                throw new Error("Invalid data type detected!")
            }

            let query = "DELETE FROM [Entry] WHERE EntryId = @EntryId";

            var request = new Request(query, function(err) {  
                if (err) {  
                   reject(err);
                   return;  
               }});  

               request.addParameter("EntryId", TYPES.BigInt, entryId);

               request.on("requestCompleted", function (rowCount, more) {
                   connection.close();
                   resolve(entryId);
               });

               connection.execSql(request);  
        }, (err) => {
            reject(err);
        });
    }, (err) => {
        reject(err);
    })
}

const getConfig = () => {
    var config = {  
        server: dbConfig.host,
        authentication: {
            type: 'default',
            options: {
                userName: dbConfig.user, 
                password: dbConfig.pwd 
            }
        },
        options: {
            encrypt: false,
            database: dbConfig.database 
        }
    }; 

    return config;
}

module.exports = {
    initialize,
    getUserInformation,
    insertUser, 
    updateUser,
    deleteUser,
    updatePassword,
    doesUsernameExist,
    getUserInformationByUsername,
    doesEmailExist,
    getUserInformationByEmail,
    doesEmailExistExcept,
    doesUsernameExistExcept,
    getEntries,
    createNewEntry,
    updateEntry,
    deleteEntry,
    doesUserIdExist
}