from langchain.graphs import Neo4jGraph

def connect_neo_graph(url: str, username: str, password: str) -> Neo4jGraph:
    return Neo4jGraph(url=url, username=username, password=password)

def create_constraints(driver: Neo4jGraph) -> None:
    driver.query(
        "CREATE CONSTRAINT question_id IF NOT EXISTS FOR (q:Question) REQUIRE (q.id) IS UNIQUE"
    )
    driver.query(
        "CREATE CONSTRAINT answer_id IF NOT EXISTS FOR (a:Answer) REQUIRE (a.id) IS UNIQUE"
    )
    driver.query(
        "CREATE CONSTRAINT user_id IF NOT EXISTS FOR (u:User) REQUIRE (u.id) IS UNIQUE"
    )
    driver.query(
        "CREATE CONSTRAINT tag_name IF NOT EXISTS FOR (t:Tag) REQUIRE (t.name) IS UNIQUE"
    )
    driver.query(
        "CREATE CONSTRAINT site_name IF NOT EXISTS FOR (s:Site) REQUIRE (s.name) IS UNIQUE"
    )