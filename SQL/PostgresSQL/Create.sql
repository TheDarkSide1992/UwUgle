CREATE TABLE IF NOT EXISTS Words (
    word_id     INT GENERATED BY DEFAULT AS IDENTITY    NOT NULL ,
    word        VARCHAR(250)        NOT NULL UNIQUE,

    PRIMARY KEY (word_id)
);

CREATE TABLE IF NOT EXISTS Files(
    file_id     INT GENERATED BY DEFAULT AS IDENTITY    NOT NULL ,
    file_name   VARCHAR(250)        NOT NULL,
    content     BYTEA,

    PRIMARY KEY (file_id)
);

CREATE TABLE IF NOT EXISTS Occurrences(
    word_id     INT                 NOT NULL,
    file_id     INT                 NOT NULL,
    count       INT                 NOT NULL,

    FOREIGN KEY (word_id) REFERENCES Words(word_id),
    FOREIGN KEY (file_id) REFERENCES Files(file_id)
);