package org.example.planningservice.commons.dtos;

import lombok.Getter;
import lombok.Setter;


public class PlanningStep1 {
    public String name;
    public String description;

    public PlanningStep1() {

    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public PlanningStep1(String name, String description) {
        this.name = name;
        this.description = description;
    }
}
