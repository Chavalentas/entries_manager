const bcrypt = require('bcryptjs');
const userConfig = require('../config/user.config.json');

const hash = async function(passWord){
    let salt = await bcrypt.genSalt(userConfig.hashingRounds);
    let hashed = await bcrypt.hash(passWord, salt);
    return hashed;
}

const isValid = async function(plainTextPassword, hashedPassword){
    let comparisonResult = await bcrypt.compare(plainTextPassword, hashedPassword);
    return comparisonResult;
}

module.exports = {
    hash,
    isValid
}
