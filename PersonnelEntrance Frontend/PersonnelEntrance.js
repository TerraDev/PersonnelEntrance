var dictionary = {}
var TobeEditedRow = 0;

window.onload = async () => {
    toastr.options["positionClass"]= "toast-bottom-left"
    try{
        DeactivateAllButtons();
        const response = await GETallEmployees()
        var success = response.status >= 200 && response.status <= 299;
        var employees = await response.json();
        //console.log(employees)
        if(success){
            employees.reverse().forEach(employee => {
                AddRow(employee.passId, employee.personnelName, employee.passTime, employee.passType);
            });
        }
        else
            toastr.error(await response.text(), "couldn't fetch data")
    }
    catch(err)
    {
        toastr.error(err, "error")
    }
    finally{
        ActivateAllButtons();
    }
    
}



AddRow = (Id, EmployeeName, PassTime, PassType) =>{
    var RowNode = Object.assign(document.createElement("form"),{className:"RowQ"})

    const FirstCell = Object.assign(document.createElement("div"),{className:"CellQ"})

    RowNode.appendChild(FirstCell);

    const DeleteButton = Object.assign(document.createElement("input"),{className:"DeleteCell", type:"button", value:"delete"});
    const EditButton = Object.assign(document.createElement("input"),{className:"EditCell", type:"button", value:"edit"});
    
    FirstCell.appendChild(DeleteButton);
    FirstCell.appendChild(EditButton);

    const SecondCell = Object.assign(document.createElement("div"),{className:"CellQ"})
    RowNode.appendChild(SecondCell);

    const SelectBox = Object.assign(document.createElement('select'),{className:"PassTypeCell", name:"PassType", disabled:"true"});
    SecondCell.appendChild(SelectBox);

    const option1 = Object.assign(document.createElement('option'),{value:'In', innerText:"وارد شد"});
    const option2 = Object.assign(document.createElement('option'),{value:'Out', innerText:"خارج شد"});
    SelectBox.appendChild(option1);
    SelectBox.appendChild(option2);
    SelectBox.value = PassType;

    const ThirdCell = Object.assign(document.createElement("div"),{className:"CellQ"})
    RowNode.appendChild(ThirdCell);

    const DateInput = Object.assign(document.createElement('input'), {className:"PassTimeCell NoDisplay", name:"PassTime", type:"datetime-local", value:PassTime,})
    const DateInfo = Object.assign(document.createElement('div'), {className:"TimeInfo", innerText:ProperDate(PassTime)})
    ThirdCell.appendChild(DateInput)
    ThirdCell.appendChild(DateInfo)
    
    const FourthCell = Object.assign(document.createElement("div"),{className:"CellQ"})
    RowNode.appendChild(FourthCell)

    const NameInput = Object.assign(document.createElement('input'), {className:"PersonnelNameCell", name:"PersonnelName", type:"text", value:EmployeeName, disabled:"true"})
    FourthCell.appendChild(NameInput) 

    RowNode.appendChild(Object.assign(document.createElement('input'),{name:"PassId" , type:"text", value:Id, hidden:"true", readOnly:"true"}));

    dictionary[Id] = RowNode;

    DeleteButton.onclick = () =>
    {
        OnclickDeleteButton(Id);
    }

    EditButton.onclick = () =>
    {
        onclickEditButton(Id);
    }

    document.getElementsByClassName("RowQ")[1].after(RowNode);
}

DeleteRow = (Id) => {
    dictionary[Id].remove();
    delete dictionary[Id];
}

EditRow = (Id, EmployeeName, PassTime, PassType) => {
    var tmpNode1 = dictionary[Id];
    tmpNode1.querySelector('.PersonnelNameCell').value = EmployeeName;
    tmpNode1.querySelector('.PassTimeCell').value = PassTime;
    tmpNode1.querySelector('.PassTypeCell').value = PassType;
    tmpNode1.querySelector('.TimeInfo').innerText = ProperDate(PassTime);
    ToggleRowActivtion(0);
}

ToggleRowActivtion = (Id) => {
    if(TobeEditedRow!=0)
        DeactivateRow(TobeEditedRow)
    if(Id != 0)
        ActivateRow(Id)
    TobeEditedRow = Id;
}

ActivateRow = (Id) =>
{
    var tmpNode = dictionary[Id]
    tmpNode.querySelector('.PersonnelNameCell').disabled = false;
    tmpNode.querySelector('.PassTimeCell').classList.remove("NoDisplay");
    tmpNode.querySelector('.PassTimeCell').disabled = false;
    tmpNode.querySelector('.PassTypeCell').disabled = false;
}

DeactivateRow = (Id) => {
    var tmpNode = dictionary[Id]
    tmpNode.querySelector('.PersonnelNameCell').disabled = true;
    //tmpNode.querySelector('.PassTimeCell').hidden = true;
    tmpNode.querySelector('.PassTimeCell').classList.add("NoDisplay");
    tmpNode.querySelector('.PassTimeCell').disabled = true;
    tmpNode.querySelector('.PassTypeCell').disabled = true;
}

ActivateAllButtons = () => {
    var AllButtons= document.querySelectorAll('input[type="button"]');
    AllButtons.forEach(button => {
        button.disabled = false;
    });
}

DeactivateAllButtons = () => {
    var AllButtons= document.querySelectorAll('input[type="button"]');
    AllButtons.forEach(button => {
        button.disabled = true;
    });
}

OnclickDeleteButton = async (id) =>
{
    try
    {
        DeactivateAllButtons();
        let response = await DELETEemployee(id);
        let success = response.status >= 200 && response.status <= 299;
        if(success)
        {
            DeleteRow(id)
            toastr.success("record successfuly deleted")
        }
        else
        {
            let responseError = await response.text()
            toastr.error(responseError,"can't delete row");
        }
    }
    catch(err)
    {
        toastr.error(err, "error")
    }
    finally{
        ActivateAllButtons();
    }
}

// submit form
onclickSubmitButton = async () => {
    try{
        var newEmployeePass = document.getElementById("AddRow");
        var PostData = 
        {
            PassId: "00000000-0000-0000-0000-000000000000",
            PersonnelName: newEmployeePass.querySelector('.PersonnelNameCell').value,
            PassTime: newEmployeePass.querySelector('.PassTimeCell').value,
            PassType: newEmployeePass.querySelector('.PassTypeCell').value,
        }
        if (PostData.PersonnelName == "")
        {
            toastr.warning("Employee name must contain a value")
            return;
        }
        else if(PostData.PassTime == "")
        {
            toastr.warning("PassTime cannot be empty")
            return ;
        }
        else if(PostData.PassType == "")
        {
            toastr.warning("PassType cannot be empty")
            return ;
        }

        // Post data
        DeactivateAllButtons();
        let response = await POSTemployee(PostData);
        let success = response.status >= 200 && response.status <= 299;
        if(success)
        {
            const resp = await response.json()
            //console.log(resp);
            AddRow(resp.passId, resp.personnelName, resp.passTime, resp.passType);
            toastr.success("Successfully added new record");
        }
        else
        {
            toastr.error(await response.text(),"Error: Cannot submit record. ")
        }
    }
    catch(err)
    {
        toastr.error(err, "error")
    }
    finally{
        ActivateAllButtons();
    }
}

var tmpEditValues = {}

onclickEditButton = async (id) => {
    try{
        if(TobeEditedRow!=id)
        {
            var oldEmployeePass = dictionary[id];
            tmpEditValues = 
            {
                PassId: oldEmployeePass.querySelector('input[name="PassId"]').value,
                PersonnelName: oldEmployeePass.querySelector('.PersonnelNameCell').value,
                PassTime: oldEmployeePass.querySelector('.PassTimeCell').value,
                PassType: oldEmployeePass.querySelector('.PassTypeCell').value,
            }

            ToggleRowActivtion(id);
            return;
        }

        var newEmployeePass = dictionary[id];

        var PutData = 
        {
            PassId: newEmployeePass.querySelector('input[name="PassId"]').value,
            PersonnelName: newEmployeePass.querySelector('.PersonnelNameCell').value,
            PassTime: newEmployeePass.querySelector('.PassTimeCell').value,
            PassType: newEmployeePass.querySelector('.PassTypeCell').value,
        }
        if (PutData.PersonnelName == "")
        {
            toastr.warning("Employee name must contain a value")
            return;
        }
        else if(PutData.PassTime == "")
        {
            toastr.warning("PassTime cannot be empty")
            return ;
        }
        else if(PutData.PassType == "")
        {
            toastr.warning("PassType cannot be empty")
            return ;
        }

        // PUT data
        DeactivateAllButtons();
        let response = await PUTemployee(PutData,id);
        let success = response.status >= 200 && response.status <= 299;
        if(success)
        {
            const resp = await response.json()
            EditRow(resp.passId, resp.personnelName, resp.passTime, resp.passType);
            toastr.success("Successfully updated the record");
        }
        else
        {
            EditRow(tmpEditValues.PassId, tmpEditValues.PersonnelName, tmpEditValues.PassTime, tmpEditValues.PassType);
            toastr.error(await response.text(), "Error: Cannot update record. Failure " );
        }
    }
    catch(err)
    {
        toastr.error(err, "error")
    }
    finally{
        ActivateAllButtons();
    }
}

//input is date
ProperDate =(date) => {
    //console.log(date);
    const DT= date.split('T');
    //console.log(DT)
    DT[0] = DT[0].replaceAll('-','/');
    DT[1] = DT[1].substring(0, 5);

    let newFormat = "در تاریخ فلان در ساعت فلان"
    newFormat = newFormat.replace('فلان', DT[0])
    newFormat = newFormat.replace('فلان', DT[1])
    //console.log(newFormat);
    return newFormat;
}