CREATE TABLE posts (
    postId INTEGER(5) PRIMARY KEY,
    title VARCHAR(30),
    creatorId TEXT REFERENCES AspNetUsers(Id),
    created DATE,
    link TEXT NOT NULL,
    score UNSIGNED INTEGER(5)
);

CREATE TABLE comments (
    commentId INTEGER(5) PRIMARY KEY,
    txt TEXT,
    creatorId TEXT REFERENCES AspNetUsers(Id),
    created DATE,
    postId INTEGER(5) REFERENCES posts,
    score UNSIGNED INTEGER(5),
    parentId INTEGER(5) REFERENCES comments(commentId)
);

CREATE TABLE user_x_post_upvoted (
    userId VARCHAR(55),
    postId INTEGER,
    upvoted BOOLEAN,
    PRIMARY KEY(userId, postId)
);

CREATE TABLE user_x_post_downvoted (
    userId VARCHAR(55),
    postId INTEGER,
    downvoted BOOLEAN,
    PRIMARY KEY(userId, postId)
);

CREATE TABLE user_x_comment_upvoted (
    userId VARCHAR(55),
    commentId INTEGER,
    upvoted BOOLEAN,
    PRIMARY KEY(userId, commentId)
);

CREATE TABLE user_x_comment_downvoted (
    userId VARCHAR(55),
    commentId INTEGER,
    downvoted BOOLEAN,
    PRIMARY KEY(userId, commentId)
);

INSERT INTO posts VALUES (0, 'Check out this search engine! (google)', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.google.de', 231);
INSERT INTO posts VALUES (1, 'Bing is the new google', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.bing.com', 421);
INSERT INTO posts VALUES (2, 'Spiegel Online', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.spiegel.de', 124);
INSERT INTO posts VALUES (3, 'Repost google', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.google.de', 15);
INSERT INTO posts VALUES (4, 'Imgur', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.imgur.com', 6);
INSERT INTO posts VALUES (5, 'posteo', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.posteo.de', 11);
INSERT INTO posts VALUES (6, 'posteo', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.posteo.de', 11);
INSERT INTO posts VALUES (61, 'posteo', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.posteo.de', 11);
INSERT INTO posts VALUES (62, 'posteo', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.posteo.de', 11);
INSERT INTO posts VALUES (53, 'posteo', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.posteo.de', 11);
INSERT INTO posts VALUES (64, 'posteo', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.posteo.de', 11);
INSERT INTO posts VALUES (66, 'posteo', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.posteo.de', 11);
INSERT INTO posts VALUES (67, 'posteo', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.posteo.de', 11);
INSERT INTO posts VALUES (58, 'posteo', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.posteo.de', 11);
INSERT INTO posts VALUES (69, 'posteo', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.posteo.de', 11);
INSERT INTO posts VALUES (699, 'posteo', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.posteo.de', 11);
INSERT INTO posts VALUES (635, 'posteo', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.posteo.de', 11);

INSERT INTO comments VALUES (0, "Awesome!", 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 0, 100);
INSERT INTO comments VALUES (1, "Awesome!", 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 0, 100);
INSERT INTO comments VALUES (2, "Awesome!", 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 0, 100);
INSERT INTO comments VALUES (3, "Awesome!", 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 1, 100);
INSERT INTO comments VALUES (4, "Awesome!", 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 1, 100);
INSERT INTO comments VALUES (5, "Awesome!", 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 2, 100);
INSERT INTO comments VALUES (6, "Awesome!", 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 0, 100);
INSERT INTO comments VALUES (7, "Awesome!", 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 0, 100);
INSERT INTO comments VALUES (8, "Awesome!", 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 0, 100);
INSERT INTO comments VALUES (9, "Awesome!", 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 0, 100);
INSERT INTO comments VALUES (10, "Awesome!", 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 0, 100);
