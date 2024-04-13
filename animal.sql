CREATE TABLE IF NOT EXISTS Animals (
                                       Id INT AUTO_INCREMENT PRIMARY KEY,
                                       Name VARCHAR(100),
                                       Category VARCHAR(50),
                                       Breed VARCHAR(50),
                                       Color VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS Visits (
                                      Id INT AUTO_INCREMENT PRIMARY KEY,
                                      VisitDate DATETIME,
                                      AnimalId INT,
                                      Description VARCHAR(255),
                                      Price DECIMAL(10, 2),
                                      FOREIGN KEY (AnimalId) REFERENCES Animals(Id)
);