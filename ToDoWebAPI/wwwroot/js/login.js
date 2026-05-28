const uri = 'api/Account';

console.log("Plik login.js został wczytany");

async function LoginToAccount()
{

    const nameInput = document.getElementById('put-name').value;
    const passwordInput = document.getElementById('put-password').value;

    const loginData = {
        Login: nameInput.trim(),
        Password: passwordInput
    };

    try {
        const response = await fetch(`${uri}/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
             body: JSON.stringify(loginData)
        })
    
        if (response.ok) {
            const user = await response.json(); 
            localStorage.setItem("token", user.token);
            window.location.href = 'todolist.html';
    } else {
        const errorText = await response.text();
        alert(errorText);
    }
} catch (error) {
    console.error("Connection error:", error);

    }


}

async function GoToRegister()
{
    window.location.href = 'register.html';
}


