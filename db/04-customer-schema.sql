CREATE SCHEMA IF NOT EXISTS customer;

CREATE TABLE customer.customer
(
    id         integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    email      text      NOT NULL,
    created_at timestamp NOT NULL DEFAULT now(),

    CONSTRAINT uq_customer_email UNIQUE (email)
);
