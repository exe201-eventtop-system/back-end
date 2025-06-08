package org.example.planningservice.commons.helper.jwt;

import io.jsonwebtoken.Claims;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureAlgorithm;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;

import java.util.Date;
import java.util.UUID;

@Component
public class JwtUtil {

    @Value("${jwt.secret}")
    private String secret;

    public Claims extractAllClaims(String token) {
        return Jwts.parser()
                .setSigningKey(secret.getBytes()) // đảm bảo dùng đúng key
                .parseClaimsJws(token)
                .getBody();
    }

    public UUID extractUserId(String token) {
        String userIdStr = extractAllClaims(token).get("Id", String.class);
        return UUID.fromString(userIdStr);
    }


    public String extractUsername(String token) {
        return extractAllClaims(token).getSubject();
    }
}