const loggerService = require('./services/loggerservice.service');
const dbService = require('./services/dbservice.service.js');
const backendConfig = require('./config/backend.config.json');
const userRoutes = require('./routes/users.route.js');
const entryRoutes = require('./routes/entries.route.js');
const express = require('express');
const cors = require('cors');
const app = express();
app.use(express.json());
app.use(cors());
app.use('/users', userRoutes);
app.use('/entries', entryRoutes);
dbService.initialize().then(() => {
    const listener = app.listen(backendConfig.port, () => {
        loggerService.logInfo('The app is listening on port ' + listener.address().port);
    });
}, (error) => {
    loggerService.logError(error);
});