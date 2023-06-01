create table note
(
    note_id     int increment primary key,
    note        int,
    bezeichnung varchar
);

create table noteArt
(
    note_art_id int increment primary key,
    bezeichnung varchar
);

create table bildungsgang
(
    bildungsgang_id int increment primary key,
    name            varchar
);

create table ausbildungsjahr
(
    ausbildungsjahr_id int increment primary key,
    bezeichnung        varchar
);

create table anmeldung
(
    anmeldung_id int increment primary key,
    benutzername varchar,
    password_hash varchar
);

create table schueler
(
    schueler_id   int increment primary key,
    fullname      varchar,
    geburtstag    date,
    anmeldung_id  int not null,
    foreign key (anmeldung_id) references anmeldung(anmeldung_id)
);

create table lehrer
(
    lehrer_id     int increment primary key,
    fullname      varchar,
    hat_administrative_rechte varchar,
    geburtstag    date,
    anmeldung_id  int not null,
    foreign key (anmeldung_id) references anmeldung(anmeldung_id)
);

create table klasse
(
    klasse_id          int increment primary key,
    klassen_name       varchar,
    ausbildungsjahr_id int,
    bildungsgang_id    int,
    lehrer_id          int,

    foreign key (ausbildungsjahr_id) references ausbildungsjahr (ausbildungsjahr_id),
    foreign key (bildungsgang_id) references bildungsgang (bildungsgang_id),
    foreign key (lehrer_id) references lehrer(lehrer_id)
);

create table schueler_hat_klasse
(
    klasse_id int,
    schueler_id int,

    foreign key (klasse_id) references klasse (klasse_id),
    foreign key (schueler_id) references schueler (schueler_id)
);

create table kurs
(
    kurs_id   int increment primary key,
    kurs_name varchar,
    lehrer_id int,

    foreign key (lehrer_id) references lehrer (lehrer_id)
);

create table klasse_hat_kurs
(
    klasse_id int,
    kurs_id   int,

    foreign key (klasse_id) references klasse (klasse_id),
    foreign key (kurs_id) references kurs (kurs_id)
);

create table leistung
(
    leistungId         int increment primary key,
    datum              date,
    notiz              varchar,
    note_id            int,
    noten_art_id       int,
    schueler_id        int,
    ausbildungsjahr_id int,
    kurs_id            int,

    foreign key (note_id) references note (note_id),
    foreign key (kurs_id) references kurs (kurs_id),
    foreign key (noten_art_id) references noteArt (note_art_id),
    foreign key (schueler_id) references schueler (schueler_id),
    foreign key (ausbildungsjahr_id) references ausbildungsjahr (ausbildungsjahr_id)
);