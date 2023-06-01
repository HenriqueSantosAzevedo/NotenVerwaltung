-- Test data for table note
INSERT INTO note (note_id, note, bezeichnung)
VALUES (1, 80, 'Mathematik'), (2, 75, 'Naturwissenschaften'), (3, 90, 'Deutsch'), (4, 85, 'Geschichte'), (5, 95, 'Geographie');

-- Test data for table noteArt
INSERT INTO noteArt (note_art_id, bezeichnung)
VALUES (1, 'Klausur'), (2, 'Hausaufgabe'), (3, 'Quiz');

-- Test data for table bildungsgang
INSERT INTO bildungsgang (bildungsgang_id, name)
VALUES (1, 'Informatik'), (2, 'Physik'), (3, 'Geschichte');

-- Test data for table ausbildungsjahr
INSERT INTO ausbildungsjahr (ausbildungsjahr_id, bezeichnung)
VALUES (1, 'Erstes Jahr'), (2, 'Zweites Jahr'), (3, 'Drittes Jahr');

-- Test data for table anmeldung
INSERT INTO anmeldung (anmeldung_id, benutzername, password_hash)
VALUES (1, 'schueler1', '7c6a180b36896a0a8c02787eeafb0e4c'), -- password: password1
       (2, 'schueler2', '7c6a180b36896a0a8c02787eeafb0e4c'), -- password: password1
       (3, 'schueler3', '7c6a180b36896a0a8c02787eeafb0e4c'), -- password: password1
       (4, 'schueler4', '7c6a180b36896a0a8c02787eeafb0e4c'), -- password: password1
       (5, 'schueler5', '7c6a180b36896a0a8c02787eeafb0e4c'), -- password: password1
       (6, 'schueler6', '7c6a180b36896a0a8c02787eeafb0e4c'), -- password: password1
       (7, 'schueler7', '7c6a180b36896a0a8c02787eeafb0e4c'), -- password: password1
       (8, 'schueler8', '7c6a180b36896a0a8c02787eeafb0e4c'), -- password: password1
       (9, 'schueler9', '7c6a180b36896a0a8c02787eeafb0e4c'), -- password: password1
       (10, 'schueler10', '7c6a180b36896a0a8c02787eeafb0e4c'), -- password: password1
       (11, 'lehrer1', '7c6a180b36896a0a8c02787eeafb0e4c'), -- password: password1
       (12, 'lehrer2', '7c6a180b36896a0a8c02787eeafb0e4c'), -- password: password1
       (13, 'lehrer3', '7c6a180b36896a0a8c02787eeafb0e4c'); -- password: password1

-- Test data for table schueler
INSERT INTO schueler (schueler_id, fullname, geburtstag, anmeldung_id)
VALUES (1, 'Max Mustermann', 1121365200, 1),
       (2, 'Sarah Schmidt', 1098306000, 2),
       (3, 'Lukas Klein', 1144635600, 3),
       (4, 'Anna Huber', 1049595600, 4),
       (5, 'Felix Meyer', 1032277200, 5),
       (6, 'Sophie Müller', 1111640400, 6),
       (7, 'Hannah Wagner', 1102683600, 7),
       (8, 'David Schneider', 1136590800, 8),
       (9, 'Julia Becker', 1049067600, 9),
       (10, 'Leon Schmitt', 1037360400, 10);


-- Test data for table lehrer
INSERT INTO lehrer (lehrer_id, fullname, hat_administrative_rechte, geburtstag, anmeldung_id)
VALUES (1, 'Herr Schmidt', 'True', 344980200, 11),
       (2, 'Frau Müller', 'False', 473222800, 12),
       (3, 'Herr Weber', 'True', 232443400, 13);


-- Test data for table klasse
INSERT INTO klasse (klasse_id, klassen_name, ausbildungsjahr_id, bildungsgang_id, lehrer_id)
VALUES (1, 'Klasse A', 1, 1, 1),
       (2, 'Klasse B', 2, 2, 2),
       (3, 'Klasse C', 3, 3, 3),
       (4, 'Klasse D', 1, 1, 2),
       (5, 'Klasse E', 2, 2, 3);

-- Test data for table schueler_hat_klasse
INSERT INTO schueler_hat_klasse (klasse_id, schueler_id)
VALUES (1, 1), (1, 2), (2, 2), (3, 3), (3, 4), (4, 5), (4, 6), (5, 7), (5, 8), (5, 9), (5, 10);

-- Test data for table kurs
INSERT INTO kurs (kurs_id, kurs_name, lehrer_id)
VALUES (1, 'Mathematik', 1),
       (2, 'Physik', 2),
       (3, 'Geschichte', 3),
       (4, 'Chemie', 1),
       (5, 'Englisch', 2);

-- Test data for table klasse_hat_kurs
INSERT INTO klasse_hat_kurs (klasse_id, kurs_id)
VALUES (1, 1), (1, 2), (2, 2), (3, 3), (3, 4), (4, 4), (5, 5);

-- Test data for table leistung
INSERT INTO leistung (leistungId, datum, notiz, note_id, noten_art_id, schueler_id, ausbildungsjahr_id, kurs_id)
VALUES
(1, 1677838800, 'Gut gemacht!', 1, 1, 1, 1, 1),
(2, 1678606800, 'Verbessere die Rechtschreibung.', 3, 2, 1, 1, 3),
(3, 1680470800, 'Sehr gute Leistung!', 5, 1, 1, 1, 5),
(4, 1677838800, 'Sehr gut!', 2, 1, 2, 2, 2),
(5, 1678606800, 'Es ist wichtig, genauer zu lesen.', 4, 2, 2, 2, 4),
(6, 1680470800, 'Tolle Arbeit!', 5, 1, 2, 2, 5),
(7, 1677838800, 'Weiter so!', 3, 1, 3, 3, 3),
(8, 1678606800, 'Sehr gut gemacht, aber noch Raum für Verbesserungen.', 4, 2, 3, 3, 4),
(9, 1680470800, 'Ausgezeichnet!', 5, 1, 3, 3, 5),
(10, 1677838800, 'Gut gemacht!', 1, 1, 4, 1, 1),
(11, 1678606800, 'Verbessere die Rechtschreibung.', 3, 2, 4, 1, 3),
(12, 1680470800, 'Sehr gute Leistung!', 5, 1, 4, 1, 5),
(13, 1677838800, 'Sehr gut!', 2, 1, 5, 2, 2),
(14, 1678606800, 'Es ist wichtig, genauer zu lesen.', 4, 2, 5, 2, 4),
(15, 1680470800, 'Tolle Arbeit!', 5, 1, 5, 2, 5),
(16, 1677838800, 'Weiter so!', 3, 1, 6, 3, 3),
(17, 1678606800, 'Sehr gut gemacht, aber noch Raum für Verbesserungen.', 4, 2, 6, 3, 4),
(18, 1680470800, 'Ausgezeichnet!', 5, 1, 6, 3, 5),
(19, 1677838800, 'Gut gemacht!', 1, 1, 7, 1, 1),
(20, 1678606800, 'Verbessere die Rechtschreibung.', 3, 2, 7, 1, 3),
(21, 1680470800, 'Sehr gute Leistung!', 5, 1, 7, 1, 5),
(22, 1677838800, 'Sehr gut!', 2, 1, 8, 2, 2),
(23, 1678606800, 'Es ist wichtig, genauer zu lesen.', 4, 2, 8, 2, 4),
(24, 1680470800, 'Tolle Arbeit!', 5, 1, 8, 2, 5),
(25, 1677838800, 'Weiter so!', 3, 1, 9, 3, 3),
(26, 1678606800, 'Sehr gut gemacht, aber noch Raum für Verbesserungen.', 4, 2, 9, 3, 4),
(27, 1680470800, 'Ausgezeichnet!', 5, 1, 9, 3, 5),
(28, 1677838800, 'Gut gemacht!', 1, 1, 10, 1, 1),
(29, 1678606800, 'Verbessere die Rechtschreibung.', 3, 2, 10, 1, 3),
(30, 1680470800, 'Sehr gute Leistung!', 5, 1, 10, 1, 5),
(31, 1677838800, 'Sehr gut!', 2, 1, 11, 2, 2),
(32, 1678606800, 'Es ist wichtig, genauer zu lesen.', 4, 2, 11, 2, 4),
(33, 1680470800, 'Tolle Arbeit!', 5, 1, 11, 2, 5),
(34, 1677838800, 'Weiter so!', 3, 1, 12, 3, 3),
(35, 1678606800, 'Sehr gut gemacht, aber noch Raum für Verbesserungen.', 4, 2, 12, 3, 4),
(36, 1680470800, 'Ausgezeichnet!', 5, 1, 12, 3, 5),
(37, 1677838800, 'Gut gemacht!', 1, 1, 13, 1, 1),
(38, 1678606800, 'Verbessere die Rechtschreibung.', 3, 2, 13, 1, 3),
(39, 1680470800, 'Sehr gute Leistung!', 5, 1, 13, 1, 5),
(40, 1677838800, 'Sehr gut!', 2, 1, 14, 2, 2),
(41, 1678606800, 'Es ist wichtig, genauer zu lesen.', 4, 2, 14, 2, 4),
(42, 1680470800, 'Tolle Arbeit!', 5, 1, 14, 2, 5),
(43, 1677838800, 'Weiter so!', 3, 1, 15, 3, 3),
(44, 1678606800, 'Sehr gut gemacht, aber noch Raum für Verbesserungen.', 4, 2, 15, 3, 4),
(45, 1680470800, 'Ausgezeichnet!', 5, 1, 15, 3, 5),
(46, 1677838800, 'Gut gemacht!', 1, 1, 16, 1, 1),
(47, 1678606800, 'Verbessere die Rechtschreibung.', 3, 2, 16, 1, 3),
(48, 1680470800, 'Sehr gute Leistung!', 5, 1, 16, 1, 5),
(49, 1677838800, 'Sehr gut!', 2, 1, 17, 2, 2),
(50, 1678606800, 'Es ist wichtig, genauer zu lesen.', 4, 2, 17, 2, 4),
(51, 1680470800, 'Tolle Arbeit!', 5, 1, 17, 2, 5);