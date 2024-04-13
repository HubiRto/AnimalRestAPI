package pl.pomoku.xd;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@Entity
@Data
@AllArgsConstructor
@NoArgsConstructor
public class Animal {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;
    private String name;
    @Enumerated(EnumType.ORDINAL)
    private AnimalCategory category;
    private String bread;
    @Enumerated(EnumType.ORDINAL)
    private Color color;
    @OneToMany(cascade = CascadeType.ALL, fetch = FetchType.EAGER)
    private List<Visit> visits;
}
