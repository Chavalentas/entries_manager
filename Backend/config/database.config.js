require('dotenv').config();
const config= {
    host: process.env.MSSQL_HOST,
    port: process.env.MSSQL_PORT,
    pwd: process.env.MSSQL_PWD,
    database: "EntriesManager",
    user: process.env.MSSQL_USER
}

module.exports = config;