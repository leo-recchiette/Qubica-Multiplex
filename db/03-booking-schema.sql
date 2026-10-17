CREATE SCHEMA IF NOT EXISTS booking;

CREATE TABLE booking.booking
(
    id          integer   GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    customer_id integer   NOT NULL,
    created_at  timestamp NOT NULL DEFAULT now()
);

CREATE TABLE booking.booking_seat
(
    id          integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    booking_id  integer NOT NULL REFERENCES booking.booking (id) ON DELETE CASCADE,
    showtime_id integer NOT NULL,
    row_label   text    NOT NULL CHECK (row_label ~ '^[A-Z]$'),
    seat_number integer NOT NULL CHECK (seat_number > 0),

    CONSTRAINT uq_booking_seat UNIQUE (showtime_id, row_label, seat_number)
);
