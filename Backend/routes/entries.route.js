const express = require('express');
const router = express.Router();
const entriesController = require('../controllers/entries.controller.js');

router.post('/create', entriesController.createAsync);
router.delete('/:entryId', entriesController.deleteAsync);
router.put('/:entryId', entriesController.updateAsync);
router.get('/:userId', entriesController.getAsync);

module.exports = router;