

// Identity Tables
Table User as U {
  Id int [pk, increment] // auto-increment
}

Table Role as R {
  Id int [pk, increment] // auto-increment
}

Table RoleClaims as RC {
  Id int [pk, increment] // auto-increment
}

Table UserRoles as UR {
  Id int [pk, increment] // auto-increment
}

Table UserClaims as UC {
  Id int [pk, increment] // auto-increment
}

Table UserLogins as UL {
  Id int [pk, increment] // auto-increment
}


//Relations between identity tables will
// be defined


// Application tables


Table Level {
  Id int [pk, increment] // auto-increment
}

Table Sublevel {
  Id int [pk, increment] // auto-increment
  LevelId int
  NumberOflessons int
}

Table GroupCondition {
  Id int [pk, increment] // auto-increment
  status int
}

Table GroupDefinition {
  Id int [pk, increment] // auto-increment
  SublevelId int
  TimeSlotId int
  PricingId int
  GroupConditionId int
  Discount double
  StartDate datetime
  EndDate datetime
  FinalTestDate datetime
  MaxGroupInstances int
  status int
  SerialNumber int
}

Table Pricing {
  Id int [pk, increment] // auto-increment
  Name varchar
  Price double
  status int
}

Table TimeSlot {
  Id int [pk, increment] // auto-increment
  Name varchar
  status int
}

Table TimeSlotDetails {
  Id int [pk, increment] // auto-increment
  TimeSlotId int
  WeekDay int 
  TimeFrom timestamp
  TimeTo timestamp
}

Table GroupInstance {
  Id int [pk, increment] // auto-increment
  GroupDefinitionId int
  status int
  SerialNumber int
} 

Table TeacherGroupInstanceAssignment {
  Id int [pk, increment] // auto-increment
  TeacherId int
  GroupInstanceId int
  IsDefault boolean
}

Table DefaultTeacherGroupDefinitionAssignment {
  Id int [pk, increment] // auto-increment
  TeacherId int
  GroupDefinitionId int
}


Table LessonDefinition {
  Id int [pk, increment] // auto-increment
  SublevelId int
  Order int
}

Table LessonTest{
  Id int [pk, increment] // auto-increment
  LessonDefinitionId int
  TestId int
}

Table SingleQuestionType {
  Id int [pk, increment] // auto-increment
  Name varchar
}

Table Choice{
  Id int [pk, increment] // auto-increment
  Name varchar
  SingleQuestionId int
}

Table ChoiceAnswer{
  Id int [pk, increment] // auto-increment
  Name varchar
  SingleQuestionId int
}


Table SingleQuestion {
  Id int [pk, increment] // auto-increment
  SingleQuestionTypeId int
  Text varchar
}

Table QuestionType {
  Id int [pk, increment] // auto-increment
  Name varchar
}

Table Question {
  Id int [pk, increment] // auto-increment
  QuestionTypeId int
  Text varchar
  MinCharacters int
  AudioPath varchar
  NoOfRepeats int
}

Table QuestionDetails {
  Id int [pk, increment] // auto-increment
  QuestionId int
  SingleQuestionId int
}

Table TestType {
  Id int [pk, increment] // auto-increment
  Name varchar
}

Table Test {
  Id int [pk, increment] // auto-increment
  Name varchar
  TestTypeId int
}

Table TestDetails {
  Id int [pk, increment] // auto-increment
  TestId int
  QuestionId int
  Order int
}

Table groupdefinitionLessonTest {
  Id int [pk, increment] // auto-increment
  GroupDefinitionId int
  LessonDefinitionId int
  TestId int
  
}

Table LessonInstance {
  Id int [pk, increment] // auto-increment
  GroupInstanceId int
  LessonDefinitionId int
  MaterialDone varchar
  MaterialToDo varchar
  HomeworkId int
}

Table LessonInstanceStudent {
  Id int [pk, increment] // auto-increment
  LessonInstanceId int
  StudentId int
  Attend boolean
  Homework boolean
}

Table Homework {
  Id int [pk, increment] // auto-increment
  Text varchar
  MinCharacters int
  Points int 
  BonusPoints int
}
  

//Example test creation

//Create a test with type Quiz and attach no of questions to it

//Create level -> level A1
//Create Sublevel for level A1 -> Sublevel A1.1
//Choose number of lessons for sublevel A1.1 -> 8
//Automatically creation of 8 lessonDefinitions for A1.1
//Assign one (or more) test(s) for lessondefinition x (from 1 to 8) for A1.1

//Create a groupdefinition for sublevel A1.1 -> G1
//Create a groupInstance -> G1-1
//Create Automatically of 8 lessonInstance (one per each LessonDefinition)
//Automatically (mayy be randomly ) choose a test


Ref: Sublevel.LevelId > Level.Id  
Ref: LessonDefinition.SublevelId > Sublevel.Id  

Ref: TimeSlotDetails.TimeSlotId > TimeSlot.Id  

Ref: GroupInstance.GroupDefinitionId > GroupDefinition.Id  

Ref: GroupDefinition.TimeSlotId > TimeSlot.Id   
Ref: GroupDefinition.PricingId > Pricing.Id   
Ref: GroupDefinition.SublevelId > Sublevel.Id  
Ref: GroupDefinition.GroupConditionId > GroupCondition.Id  

Ref: TeacherGroupInstanceAssignment.GroupInstanceId > GroupInstance.Id
Ref: TeacherGroupInstanceAssignment.TeacherId > U.Id

Ref: LessonInstance.GroupInstanceId > GroupInstance.Id  
Ref: LessonInstance.LessonDefinitionId > LessonDefinition.Id  

Ref: DefaultTeacherGroupDefinitionAssignment.GroupDefinitionId > GroupDefinition.Id
Ref: DefaultTeacherGroupDefinitionAssignment.TeacherId > U.Id


Ref: "TimeSlot"."Id" < "TimeSlotDetails"."TimeTo"

Ref: "GroupInstance"."Id" < "GroupDefinition"."TimeSlotId"

// 1) Grading (before tests + After tests)
// 2) Review lessoninstace Table with its subtables (LessonInstanceStudent,Homework)