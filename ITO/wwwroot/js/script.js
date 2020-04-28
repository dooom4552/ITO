function loginValidate(){
     var username = document.getElementById("username").value;
     var password = document.getElementById("psw").value;
    if (username == "" || password == "")
    {
        alert ("Введите имя пользователя и пароль");
    }
}