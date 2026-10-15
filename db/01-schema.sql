CREATE SCHEMA IF NOT EXISTS catalog;

CREATE TABLE catalog.director
(
    id         integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    first_name text NOT NULL,
    last_name  text NOT NULL,

    CONSTRAINT uq_director_name UNIQUE (first_name, last_name)
);

CREATE TABLE catalog.genre
(
    id   integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name text NOT NULL,

    CONSTRAINT uq_genre_name UNIQUE (name)
);

CREATE TABLE catalog.movie
(
    id               integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    title            text    NOT NULL,
    description      text    NOT NULL,
    director_id      integer NOT NULL REFERENCES catalog.director (id),
    genre_id         integer NOT NULL REFERENCES catalog.genre (id),
    duration_minutes integer NOT NULL CHECK (duration_minutes > 0),
    release_date     date    NOT NULL
);

CREATE TABLE catalog.hall
(
    id   integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name text NOT NULL,

    CONSTRAINT uq_hall_name UNIQUE (name)
);

CREATE TABLE catalog.showtime
(
    id        integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    movie_id  integer   NOT NULL REFERENCES catalog.movie (id),
    hall_id   integer   NOT NULL REFERENCES catalog.hall (id),
    starts_at timestamp NOT NULL
);
