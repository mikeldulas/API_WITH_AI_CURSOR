CREATE TABLE IF NOT EXISTS biodata (
    id UUID PRIMARY KEY,
    nama VARCHAR(150) NOT NULL,
    tanggal_lahir DATE NOT NULL,
    alamat TEXT,
    no_hp VARCHAR(20),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW()
);
