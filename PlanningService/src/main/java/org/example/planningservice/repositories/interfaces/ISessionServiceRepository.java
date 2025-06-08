package org.example.planningservice.repositories.interfaces;

import org.example.planningservice.bo.entities.SessionService;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

public interface ISessionServiceRepository extends JpaRepository<SessionService, UUID> {}
