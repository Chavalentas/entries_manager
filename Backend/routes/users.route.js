const express = require('express');
const router = express.Router();
const usersController = require('../controllers/users.controller.js');

router.post('/userid', usersController.verifyUserAsync);
router.post('/login', usersController.loginAsync);
router.post('/register', usersController.registerAsync);
router.delete('/:userId', usersController.deleteAsync);
router.put('/:userId', usersController.updateAsync);
router.put('/pwd/:userId', usersController.updatePasswordAsync);
router.get('/:userId', usersController.getUserInformationAsync);

module.exports = router;