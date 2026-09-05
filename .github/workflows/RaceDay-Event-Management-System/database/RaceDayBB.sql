CREATE TABLE Result(
Result_ID INT PRIMARY KEY,
Enrolment_ID INT NOT NULL,
Finish_Time TIME NOT NULL,
Enrolment_Date DATE NOT NULL DEFAULT GETDATE(),

FOREIGN KEY (Enrolment_ID) REFERENCES Enrolment(Enrolment_ID),

);