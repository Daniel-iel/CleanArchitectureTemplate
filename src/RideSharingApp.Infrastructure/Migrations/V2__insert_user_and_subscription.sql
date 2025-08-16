-- Inserir um usuário válido
INSERT INTO Users (id, email, password_hash) VALUES (
  'ce7a2760-364e-4d77-b734-5d0070030ec5',
  'iel_182@hotmail.com',
  '0!d4#TY@IEtLWw@TuYzE%H!@tDuLKp' -- hash de exemplo
);

-- Inserir uma assinatura válida para o usuário
INSERT INTO Subscriptions (id, user_id, start_date, end_date) VALUES (
  '5c4de758-8b20-462d-91b2-db78bce25d72',
  'ce7a2760-364e-4d77-b734-5d0070030ec5',
  NOW(),
  NOW()
);
