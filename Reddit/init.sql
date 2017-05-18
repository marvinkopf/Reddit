CREATE TABLE posts (
    postId INTEGER PRIMARY KEY AUTOINCREMENT,
    title VARCHAR(30),
    creatorId TEXT REFERENCES AspNetUsers(Id),
    created DATE,
    link TEXT NOT NULL,
    score UNSIGNED INTEGER(5),
    subreddit VARCHAR(30) REFERENCES subreddits(name),
    urlToImage VARCHAR(55)
);

CREATE TABLE subreddits (
    name VARCHAR(30) PRIMARY KEY
);

CREATE TABLE comments (
    commentId INTEGER PRIMARY KEY AUTOINCREMENT,
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

INSERT INTO subreddits VALUES ("news");
INSERT INTO subreddits VALUES ("google-news");
INSERT INTO subreddits VALUES ("abc-news-au");
INSERT INTO subreddits VALUES ("bbc-sport");

INSERT INTO posts VALUES (4, 'Imgur', 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 'www.imgur.com', 6, 'news', '');
INSERT INTO comments VALUES (0, "Awesome!", 'f6b51ae3-4ae4-49a7-acdb-9c06fd7bd44c', CURRENT_DATE, 0, 0, NULL);
