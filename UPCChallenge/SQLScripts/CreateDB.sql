use master
go
create database DBMatricula
go
use DBMatricula
go

create table Alumno
(IdAlumno int identity(1,1) primary key not null,
Nombres varchar(50) not null,
Apellidos varchar(50) not null,
)

create table Cursos
(IdCurso int identity(1,1) primary key not null,
 Nombre varchar(60) not null,
 Descripcion varchar(70) null,
 NroCreditos varchar(12) not null,
 Activo bit not null
)

create table Solicitud
(IdSolicitud int identity(1,1) primary key not null,
 IdAlumno int foreign key references Alumno not null,
 FechaSolicitud DateTime not null,
 CodRegistrante varchar(10) null,
 Carrera varchar(12) not null,
 Periodo varchar(7) not null
 )

 create table DetalleSolicitud
(IdDetalleSol int identity(1,1) primary key not null,
 IdSolicitud int foreign key references Solicitud not null,
 IdCurso int foreign key references Cursos not null,
 Profesor varchar(60) not null,
 Aula varchar(12) not null,
 Sede varchar(20) not null,
 Observaciones varchar(20) null
 )
