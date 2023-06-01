INSERT INTO note
VALUES (1, 1, 'Sehr Gut'),
       (2, 2, 'Gut'),
       (3, 3, 'Befriedigend'),
       (4, 4, 'Ausreichend'),
       (5, 5, 'Mangelhaft'),
       (6, 6, 'Ungenügend');

INSERT INTO noteArt
VALUES (1, 'Sonstige mitarbeit'),
       (2, 'Zwischen Zeugnis'),
       (3, 'Abschlusszeugnis');

INSERT INTO anmeldung VALUES (1, 'jmehle', 'test'), (2, 'mebers', 'test2');

INSERT INTO schueler VALUES (1, 'Jürgen Mehler', 963187200000, 1);
INSERT INTO lehrer VALUES (1, 'Markus Ebersbach', 'false', 963187200000, 2);

INSERT INTO ausbildungsjahr
VALUES (1, '2021');

INSERT INTO bildungsgang
VALUES (1, 'Anwendungsentwickler');

INSERT INTO klasse
VALUES (1, 'IA121', 1, 1, 1);

INSERT INTO schueler_hat_klasse
VALUES (1, 1);

INSERT INTO kurs
VALUES (1, 'Git', 1);

INSERT INTO klasse_hat_kurs
VALUES (1, 1);

INSERT INTO leistung
VALUES (1, 1678665600000, '', 2, 2, 1, 1);