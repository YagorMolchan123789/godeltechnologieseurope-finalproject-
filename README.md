# Introduction 
Medical center

# How to run it via docker-compose
0. Open up the folder containing readme.md and docker-compose.yml with vs code, powershell or cmd.
1. Run `docker compose -p medical_center build`
If you have an error please contact someone from teh team.
2. Run `docker compose -p medical_center up`
If you have an error please contact someone from teh team.
3. Now, you should see inside your docker desktop container group with name medical_center. One of them (migrations) will be failed. It's ok.

# How to re-run migrations if you removed DB etc
Just run migrations container from your docker desktop application