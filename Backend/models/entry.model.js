class Entry{
    constructor(entryId, entryText, entryTitle, entryDate, userId){
        this.entryId = entryId;
        this.entryTitle = entryTitle;
        this.entryText = entryText;
        this.entryDate = entryDate;
        this.userId = userId;
    }
}

module.exports = Entry;