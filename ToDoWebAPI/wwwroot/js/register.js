const uri = 'api/Account';


async function RegisterAccount()
{
    const RegisterName = document.getElementById('put-name').value;
    const RegisterPassword = document.getElementById('put-password').value;

    if (RegisterName === "" || RegisterPassword === "") {
        alert("Login and password can't be empty!");
        return; }

    if (RegisterName.length < 3) {
        alert("Login needs to have at least 3 characters!");
        return;
    }

    if (RegisterPassword.length < 3) {
        alert("Password needs to have at least 3 characters!");
        return; }

    const registerData = {
        Login: RegisterName.trim(),
        Password: RegisterPassword };

    try {
        const response = await fetch(`${uri}/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(registerData) 
        })

        if (response.ok) {
            const data = await response.json(); 
            localStorage.setItem("token", data.token);
            alert("Account made successfully! Welcome to the todo app :D");
            window.location.href = 'todolist.html';
        } else {
            const errorText = await response.text();
            alert("Error: " + errorText); 
        }
    } catch (error) {
        console.error("Registration error", error);

    }


}