package org.example.planningservice.bo.entities;

import jakarta.persistence.*;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.time.LocalDateTime;
import java.util.UUID;

@Setter
@Getter
@Entity
@NoArgsConstructor
@Table(name = "session_service")
public class SessionService {
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    @Column(name = "id", nullable = false)
    private UUID id;

    @ManyToOne
    @JoinColumn(name = "planning_session_id", nullable = false)
    private PlanningSession planningSession;

    @Column(name = "service_id")
    private UUID serviceId;

    @Column(name = "create_at")
    private LocalDateTime createAt;

    @Column(name = "update_date")
    private LocalDateTime updateDate;

}