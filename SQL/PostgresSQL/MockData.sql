
INSERT INTO Words (word) values ('MOCK');
INSERT INTO Words (word) values ('Arch');
INSERT INTO Words (word) values ('Linux');
INSERT INTO Words (word) values ('Docker');
INSERT INTO Words (word) values ('Ubuntu');

INSERT INTO Files(file_name) values ('FileMock');
INSERT INTO Files(file_name) values ('Yahoo');
INSERT INTO Files(file_name) values ('UwUgle');

INSERT INTO Occurrences(word_id, file_id, count) values (1,1,12);
INSERT INTO Occurrences(word_id, file_id, count) values (2,1,1);
INSERT INTO Occurrences(word_id, file_id, count) values (2,2,1);
INSERT INTO Occurrences(word_id, file_id, count) values (3,1,7);
INSERT INTO Occurrences(word_id, file_id, count) values (3,2,11);
INSERT INTO Occurrences(word_id, file_id, count) values (4,1,9);
INSERT INTO Occurrences(word_id, file_id, count) values (4,2,1);
INSERT INTO Occurrences(word_id, file_id, count) values (5,3,1);
