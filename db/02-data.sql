INSERT INTO catalog.director (first_name, last_name)
VALUES ('Peter', 'Jackson'),
       ('Quentin', 'Tarantino'),
       ('Richard', 'Linklater'),
       ('Chan-wook', 'Park'),
       ('Hayao', 'Miyazaki'),
       ('Joon-ho', 'Bong');

INSERT INTO catalog.genre (name)
VALUES ('Fantasy'),
       ('Action'),
       ('Drama'),
       ('Thriller'),
       ('Animation'),
       ('Science Fiction');

INSERT INTO catalog.movie (title, description, director_id, genre_id, duration_minutes, release_date)
VALUES ('The Lord of the Rings: The Fellowship of the Ring',
        'A young hobbit, Frodo, inherits the One Ring and sets out with eight companions to destroy it in the fires of Mount Doom.',
        (SELECT id FROM catalog.director WHERE first_name = 'Peter' AND last_name = 'Jackson'),
        (SELECT id FROM catalog.genre WHERE name = 'Fantasy'),
        178, '2001-12-19'),

       ('The Lord of the Rings: The Two Towers',
        'The Fellowship is broken: Frodo and Sam head for Mordor guided by Gollum, while Aragorn, Legolas and Gimli help Rohan stand against Saruman''s army.',
        (SELECT id FROM catalog.director WHERE first_name = 'Peter' AND last_name = 'Jackson'),
        (SELECT id FROM catalog.genre WHERE name = 'Fantasy'),
        179, '2002-12-18'),

       ('The Lord of the Rings: The Return of the King',
        'As Gondor holds out against Sauron''s assault, Frodo and Sam face the last stretch of their journey to Mount Doom.',
        (SELECT id FROM catalog.director WHERE first_name = 'Peter' AND last_name = 'Jackson'),
        (SELECT id FROM catalog.genre WHERE name = 'Fantasy'),
        201, '2003-12-17'),

       ('Kill Bill: Volume 1',
        'Waking from a coma, the Bride sets out for revenge on the gang of assassins who tried to kill her on her wedding day.',
        (SELECT id FROM catalog.director WHERE first_name = 'Quentin' AND last_name = 'Tarantino'),
        (SELECT id FROM catalog.genre WHERE name = 'Action'),
        111, '2003-10-10'),

       ('Kill Bill: Volume 2',
        'The Bride continues her revenge against the remaining members of the gang, leading to a final showdown with Bill.',
        (SELECT id FROM catalog.director WHERE first_name = 'Quentin' AND last_name = 'Tarantino'),
        (SELECT id FROM catalog.genre WHERE name = 'Action'),
        137, '2004-04-16'),

       ('Blue Moon',
        'New York, 1943: on the opening night of Oklahoma!, lyricist Lorenz Hart faces the end of his partnership with Richard Rodgers.',
        (SELECT id FROM catalog.director WHERE first_name = 'Richard' AND last_name = 'Linklater'),
        (SELECT id FROM catalog.genre WHERE name = 'Drama'),
        100, '2025-10-17'),

       ('Oldboy',
        'Oh Dae-su is kidnapped and imprisoned for fifteen years without knowing why; once released, he has five days to find out who locked him up.',
        (SELECT id FROM catalog.director WHERE first_name = 'Chan-wook' AND last_name = 'Park'),
        (SELECT id FROM catalog.genre WHERE name = 'Thriller'),
        120, '2003-11-21'),

       ('Spirited Away',
        'While moving to a new home, ten-year-old Chihiro wanders into a world of spirits and must work in a bathhouse to save her parents, who have been turned into pigs.',
        (SELECT id FROM catalog.director WHERE first_name = 'Hayao' AND last_name = 'Miyazaki'),
        (SELECT id FROM catalog.genre WHERE name = 'Animation'),
        125, '2001-07-20'),

       ('Mickey 17',
        'Mickey is an "Expendable": a member of a space colony mission who is reprinted in a new body every time he dies.',
        (SELECT id FROM catalog.director WHERE first_name = 'Joon-ho' AND last_name = 'Bong'),
        (SELECT id FROM catalog.genre WHERE name = 'Science Fiction'),
        137, '2025-03-07');

INSERT INTO catalog.hall (name, row_count, seats_per_row)
VALUES ('Hall 1', 8, 30),
       ('Hall 2', 6, 20),
       ('Hall 3', 5, 12);

INSERT INTO catalog.showtime (movie_id, hall_id, starts_at)
VALUES ((SELECT id FROM catalog.movie WHERE title = 'The Lord of the Rings: The Fellowship of the Ring'), (SELECT id FROM catalog.hall WHERE name = 'Hall 2'), '2026-10-20 17:00'),
       ((SELECT id FROM catalog.movie WHERE title = 'The Lord of the Rings: The Fellowship of the Ring'), (SELECT id FROM catalog.hall WHERE name = 'Hall 3'), '2026-10-21 20:30'),

       ((SELECT id FROM catalog.movie WHERE title = 'The Lord of the Rings: The Two Towers'), (SELECT id FROM catalog.hall WHERE name = 'Hall 3'), '2026-10-20 20:30'),
       ((SELECT id FROM catalog.movie WHERE title = 'The Lord of the Rings: The Two Towers'), (SELECT id FROM catalog.hall WHERE name = 'Hall 1'), '2026-10-22 17:00'),

       ((SELECT id FROM catalog.movie WHERE title = 'The Lord of the Rings: The Return of the King'), (SELECT id FROM catalog.hall WHERE name = 'Hall 2'), '2026-10-21 17:00'),
       ((SELECT id FROM catalog.movie WHERE title = 'The Lord of the Rings: The Return of the King'), (SELECT id FROM catalog.hall WHERE name = 'Hall 3'), '2026-10-22 20:30'),

       ((SELECT id FROM catalog.movie WHERE title = 'Kill Bill: Volume 1'), (SELECT id FROM catalog.hall WHERE name = 'Hall 3'), '2026-10-20 18:00'),
       ((SELECT id FROM catalog.movie WHERE title = 'Kill Bill: Volume 1'), (SELECT id FROM catalog.hall WHERE name = 'Hall 2'), '2026-10-21 22:00'),

       ((SELECT id FROM catalog.movie WHERE title = 'Kill Bill: Volume 2'), (SELECT id FROM catalog.hall WHERE name = 'Hall 2'), '2026-10-20 22:00'),
       ((SELECT id FROM catalog.movie WHERE title = 'Kill Bill: Volume 2'), (SELECT id FROM catalog.hall WHERE name = 'Hall 3'), '2026-10-21 18:00'),

       ((SELECT id FROM catalog.movie WHERE title = 'Blue Moon'), (SELECT id FROM catalog.hall WHERE name = 'Hall 1'), '2026-10-20 19:00'),
       ((SELECT id FROM catalog.movie WHERE title = 'Blue Moon'), (SELECT id FROM catalog.hall WHERE name = 'Hall 2'), '2026-10-22 21:00'),

       ((SELECT id FROM catalog.movie WHERE title = 'Oldboy'), (SELECT id FROM catalog.hall WHERE name = 'Hall 1'), '2026-10-20 21:30'),
       ((SELECT id FROM catalog.movie WHERE title = 'Oldboy'), (SELECT id FROM catalog.hall WHERE name = 'Hall 1'), '2026-10-22 22:00'),

       ((SELECT id FROM catalog.movie WHERE title = 'Spirited Away'), (SELECT id FROM catalog.hall WHERE name = 'Hall 1'), '2026-10-20 16:00'),
       ((SELECT id FROM catalog.movie WHERE title = 'Spirited Away'), (SELECT id FROM catalog.hall WHERE name = 'Hall 1'), '2026-10-21 16:00'),

       ((SELECT id FROM catalog.movie WHERE title = 'Mickey 17'), (SELECT id FROM catalog.hall WHERE name = 'Hall 1'), '2026-10-21 21:00'),
       ((SELECT id FROM catalog.movie WHERE title = 'Mickey 17'), (SELECT id FROM catalog.hall WHERE name = 'Hall 2'), '2026-10-22 18:30');
