class User{
    constructor(userId, firstName, lastName, userName, email, password){
        this.userId = userId;
        this.firstName = firstName;
        this.lastName = lastName;
        this.userName = userName;
        this.email = email;
        this.password = password;
    }
}

module.exports = User;