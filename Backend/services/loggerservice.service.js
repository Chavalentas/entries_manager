const logInfo = function(message){
    var date = new Date();
    var dateString = `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
    var timeString = date.toLocaleTimeString();
    console.info('\x1b[33m%s\x1b[0m', `[${dateString} ${timeString}] ${message}`);
}

const logError = function(error){
    var date = new Date();
    var dateString = `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
    var timeString = date.toLocaleTimeString();
    console.info('\x1b[31m%s\x1b[0m', `[${dateString} ${timeString}] ${error}`);
}

module.exports = {
    logInfo,
    logError
}
