const uri = 'api/ToDos';
let activeTodosGlobal = [];
let historyTodosGlobal = [];

function welcomeUser() {
    const token = localStorage.getItem('token');
    const displayElement = document.getElementById('display-user');

    if (!token || !displayElement) return;

    try {
        const base64Payload = token.split('.')[1];
        const decodedPayload = atob(base64Payload);
        const payloadData = JSON.parse(decodedPayload);
        const username = payloadData["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
        if (username) {
            displayElement.innerText = `${username}!`;
        }
    } catch (error) {
        console.error("Failed to decode token for welcome message:", error);
    }
}
function _displayCount(todoCount) {
    const name = (todoCount === 1) ? 'todo' : 'todoes';
    const counterEl = document.getElementById('counter');
    if (counterEl) counterEl.innerText = `${todoCount} ${name}`;
}


///////////////////////////////////////////////////
// todos
function getTodos() {
    const token = localStorage.getItem("token");

    fetch(`${uri}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
        })
        .then(response => {
            if (response.status === 401) { 
                window.location.href = "login.html";
                return;
            }
            if (!response.ok) {
                return response.text().then(text => { throw new Error(text) });
            }
            return response.json();
        })
        .then(data => {
            if (data) {
                console.log('List data:', data);
                _displayTodos(data);
            }
        })
        .catch(error => console.error('Unable to get todoes', error));

}

function _displayTodos(data) {
    const tBody = document.getElementById('todolist');
    tBody.innerHTML = '';
    const activeTodos = data.filter(t => !t.done);
    _displayCount(activeTodos.length);
    activeTodos.forEach(todo => {

        let tr = tBody.insertRow();

        tr.insertCell(0).innerText = todo.name; 
        tr.insertCell(1).innerText = todo.done ? "Finished" : "To do";
        tr.insertCell(2).innerText = todo.dateTo;

        let tdAction = tr.insertCell(3); 
        let editButton = document.createElement('button');
        editButton.innerText = 'edit';
        editButton.onclick = () => displayEditForm(todo.id);

        let deleteButton = document.createElement('button');
        deleteButton.innerText = 'delete';
        deleteButton.onclick = () => deleteTodo(todo.id); 

        tdAction.appendChild(editButton);
        tdAction.appendChild(deleteButton);
         
    });

    activeTodosGlobal = data; 
}

///////////////////////////////////////////////////
// history
function getHistory() {
    const token = localStorage.getItem("token");

    fetch(`${uri}/history`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
        })
        .then(response => {
            if (response.status === 401) { 
                window.location.href = "login.html";
                return;
            }
            if (!response.ok) {
                return response.text().then(text => { throw new Error(text) });
            }
            return response.json();
        })
        .then(data => {
            if (data) {
                console.log('History data:', data);
                _displayHistory(data);
            }
        })
        .catch(error => console.error('Unable to get history', error));
}

function _displayHistory(data) {
    const tBody = document.getElementById('history'); 
    tBody.innerHTML = '';

    data.forEach(todo => {
        let tr = tBody.insertRow();
        tr.insertCell(0).innerText = todo.name;
        tr.insertCell(1).innerText = todo.done ? "Finished" : "Overdue";
        tr.insertCell(2).innerText = todo.dateTo;
        let tdAction = tr.insertCell(3);
        if (todo.done === true) {
            tdAction.innerText = "No actions avaliable";
        } else {
            let editButton = document.createElement('button');
            editButton.innerText = 'edit';
            editButton.onclick = () => displayEditForm(todo.id);

            let deleteButton = document.createElement('button');
            deleteButton.innerText = 'delete';
            deleteButton.onclick = () => deleteTodo(todo.id);

            tdAction.appendChild(editButton);
            tdAction.appendChild(deleteButton);
        }

    });

    historyTodosGlobal = data;
}


///////////////////////////////////////////////////
// add todo
function addTodo() {
    const token = localStorage.getItem("token");

    const addNameTextbox = document.getElementById('add-name');
    const addDateTextbox = document.getElementById('add-date');
    const errorSpan = document.getElementById('add-name-error');

    var n = addNameTextbox.value.trim();
    if (!n) {
        errorSpan.style.display = 'block'; 
        return; 
    } else {
        errorSpan.style.display = 'none'; 
    }

    const todo = {
        name: n,
        dateTo: addDateTextbox.value,
    };

    fetch(`${uri}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(todo)
    })
        .then(response =>
        {
            if (!response.ok) {
                return response.text().then(text => { throw new Error(text) });
            }
            return response.json();
        })
        .then(() => {
            refreshData(); 
            addNameTextbox.value = '';
            addDateTextbox.value = '';
        })
        .catch(error => console.error('Unable to add todo', error));
}

///////////////////////////////////////////////////
// delete todo
function deleteTodo(id) {
    const token = localStorage.getItem("token");

    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`
        }
        })
        .then(response => {
            if (!response.ok) { 
                throw new Error('Couldn\'t delete todo');
            }
            console.log('Todo deleted, refreshing data');
            setTimeout(() => {
                refreshData();
            }, 100);

        })
        .catch(error => console.error('Unable to delete todo', error));
}

///////////////////////////////////////////////////
// edit
function displayEditForm(id) {
    const todo = activeTodosGlobal.find(t => t.id === id) || historyTodosGlobal.find(t => t.id === id);
    if (!todo) return;
    document.getElementById('edit-id').value = todo.id; 
    document.getElementById('edit-name').value = todo.name;
    document.getElementById('edit-date').value = todo.dateTo; 
    document.getElementById('edit-done').checked = todo.done;
    document.getElementById('editForm').style.display = 'block';
}
function editTodo() {
    const token = localStorage.getItem("token");

    const todoId = parseInt(document.getElementById('edit-id').value, 10);
    const name = document.getElementById('edit-name');
    const dateTo = document.getElementById('edit-date');
    const checkbox = document.getElementById('edit-done');

    const todo = {
        id: todoId, 
        name: name.value.trim(),
        dateTo: dateTo.value,
        done: checkbox.checked
    };

    fetch(`${uri}/${todoId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json', 
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(todo)
        })
        .then(response => {
            if (!response.ok) {
                return response.text().then(text => { throw new Error(text) });
            }
            refreshData();
            closeInput();
        })
        .catch(error => console.error('Unable to update todo', error));

    closeInput();

    return false;
}



function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function logOut() {
    localStorage.removeItem("token");
    window.location.href = "login.html";
}

function refreshData() {
    getTodos();
    getHistory();
}


document.addEventListener('DOMContentLoaded', () => { 
    welcomeUser();
    refreshData();
    setInterval(refreshData, 30000);
});