package org.example.planningservice.api.config;
import io.swagger.v3.oas.models.info.Info;
import io.swagger.v3.oas.models.info.Contact;
import io.swagger.v3.oas.models.OpenAPI;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class OpenApiConfig {

    @Bean
    public OpenAPI customOpenAPI() {
        return new OpenAPI()
                .info(new Info()
                        .title("EventTop API")
                        .version("1.0")
                        .description("API cho hệ thống đặt lịch sự kiện")
                        .contact(new Contact().name("Dev Team").email("dev@eventtop.com")));
    }
}
