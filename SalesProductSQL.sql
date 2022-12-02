create database SalesProduct

use SalesProduct

create table Member (
	MemberId int identity(1,1) not null primary key,
	MemberName varchar(100) not null,
	Email varchar(100) not null,
	CompanyName varchar(40) not null,
	City varchar(15) not null,
	Country varchar(15) not null,
	Password varchar(30) not null,
	IsAdmin bit not null
)

create table "Order" (
	OrderId int identity(1,1) not null primary key,
	MemberId int not null references Member(MemberId),
	OrderDate datetime not null,
	RequiredDate datetime,
	ShippedDate datetime,
	Freight money
)

create table Product (
	ProductId int identity(1,1) not null primary key,
	CategoryId int not null,
	ProductName varchar(40) not null,
	Weight varchar(20) not null,
	UnitPrice money not null,
	UnitInStock int not null
)

create table OrderDetail (
	ProductId int not null references Product(ProductId),
	OrderId int not null references "Order"(OrderId),
	ProductName varchar(40) not null,
	UnitPrice money not null,
	Quantity int not null,
	Discount float not null,
	primary key (ProductId, OrderId)
)

insert into Member values ('Nguyen Ngoc Thinh', 'thinh@gmail.com', 'Bosch', 'HCM', 'Viet Nam', '123', 1)
insert into Member values ('Nguyen Ba Nhat Tai', 'tai@gmail.com', 'KMS Solution', 'Ben Tre', 'Viet Nam', '123', 0)

insert into Product values ('1', 'Desk', '2', 100000, 20)
insert into Product values ('2', 'Table', '4', 200000, 20)
insert into Product values ('2', 'Fan', '3', 300000, 10)

insert into "Order" values (2, GETDATE(), GETDATE() + 1, GETDATE() + 2, 20000)
insert into "Order" values (2, GETDATE(), GETDATE() + 2, GETDATE() + 3, 20000)

insert into OrderDetail values (1, 1, 'Desk', 100000, 5, 2)
insert into OrderDetail values (2, 2, 'Table', 200000, 5, 1)
insert into OrderDetail values (3, 2, 'Fan', 300000, 1, 0)