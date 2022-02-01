GETallEmployees = async () => {
    return await fetch("https://localhost:44316/api/PersonnelPass");
}

POSTemployee = async (EmployeePass) => {
    //console.log(EmployeePass)
    return await fetch('https://localhost:44316/api/PersonnelPass', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(EmployeePass)
      });
}

PUTemployee = async (EmployeePass,id) => {
    //console.log(EmployeePass)
    //console.log(id)
    return await fetch("https://localhost:44316/api/PersonnelPass/"+id,{
        method: 'PUT', // Method itself
        headers: {
         'Content-type': 'application/json;' // Indicates the content 
        },
        body: JSON.stringify(EmployeePass) // We send data in JSON format
    })
}

DELETEemployee = async (id) => {
    //console.log(id)
    //console.log(typeof id)
    return await fetch('https://localhost:44316/api/PersonnelPass/'+id , {
        method: 'DELETE',
        headers: {
            'Content-type': 'application/json'
        }
    })
}