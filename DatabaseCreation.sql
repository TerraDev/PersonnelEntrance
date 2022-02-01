

Create database PersonnelPassDB

go
use PersonnelPassDB
create table PassType
(
PassTypeId int not null identity(1,1),
PassTypeName nvarchar(20) not null,
Constraint PK_Constraint primary key (PassTypeId),
Constraint Unique_Constraint Unique (PassTypeName)
);

insert into PassType(PassTypeName)
values ('In'),('Out');

create table PersonnelPass
(
PassId UniqueIdentifier not null default NewId(),
EmployeeName Nvarchar(32) not null,
PassTime Datetime2 not null,
PassType int not null,
Constraint PK_Constraint_PersonnelPass Primary key (PassId),
Constraint Unique_Constraint_PersonnelPass Unique (EmployeeName,PassTime),
Constraint FK_Constraint_PersonnelPass Foreign key (PassType) references PassType(PassTypeId),
);

/*insert into PersonnelPass(PassId, EmployeeName, PassTime, PassType)
values(default,'Ali','2020-04-14 08:59',1);
('Reza',format(getdate(),'yyyy-mm-dd hh:mm'),1),
('Ahmad','2020/03/12 23:10',2);*/

/*drop database PersonnelPassDB*/


/* Create triggers: */

go
create trigger CountInsertFilter
on PersonnelPass
instead of insert
as
begin tran mytran
save tran mytran
	insert into PersonnelPass
	select * from inserted i;

	declare @counter int;
	declare @Empname nvarchar(32);

	set @Empname = (select EmployeeName from inserted);

	set @counter = (Select count(*) 
	from PersonnelPass p join inserted i
	on p.EmployeeName = i.EmployeeName)

	if (@counter > 4)
		begin
			declare @errormsg nvarchar(100);
			set @errormsg = 'Cannot insert record for ' + @Empname + ', because 4 rows records exist for this Employee.'
			raiserror(@errormsg ,16,50000);
			rollback tran mytran;
		end
Commit tran mytran;

go

create trigger CountUpdateFilter
on PersonnelPass
instead of update
as
begin tran mytran
save tran mytran
	update PersonnelPass
	set EmployeeName = i.EmployeeName, PassTime=i.PassTime, PassType=i.PassType
	from PersonnelPass inner join inserted i on i.PassId = PersonnelPass.PassId;

	declare @counter int;
	declare @Empname nvarchar(32);
	set @Empname = (Select EmployeeName from inserted)

	set @counter = (Select count(*) 
	from PersonnelPass p join inserted i
	on p.EmployeeName = i.EmployeeName)

	if (@counter > 4)
		begin
			declare @errormsg2 nvarchar(100);
			set @errormsg2 = 'Cannot update record for ' + @Empname + ', because 4 rows records exist for this Employee.'
			raiserror(@errormsg2 ,16,50000);
			rollback tran mytran;
		end
Commit tran mytran;

go