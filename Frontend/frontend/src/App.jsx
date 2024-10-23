import { useEffect, useState } from "react";
import { API_URL } from "./consts";

function App() {
  const [dogs, setDogs] = useState([]);
  const [isFetching, setIsFetching] = useState(true);
  const [newTaskTitle, setNewTaskTitle] = useState("");

  // LOAD EXISTING DOGS
  useEffect(() => {
    fetch(`${API_URL}/dogs`)
      .then((response) => response.json())
      .then((data) => {
        setDogs(data);
        setIsFetching(false);
      });
  }, []);

  // HANDLE UPDATING DOG
  const handleCheckedChangeRequest = (id, checked) => {
    fetch(`${API_URL}/dogs/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ completed: checked }),
    })
      .then((response) => {
        if (!response.ok) {
          setIsFetching(false);
          throw new Error("Failed to update dog");
        }
      })
      .then(() => {
        setDogs((prevDogs) =>
          prevDogs.map((dog) => {
            if (dog.id === id) {
              return { ...dog, completed: checked };
            }
            return dog;
          })
        );
        setIsFetching(false);
      })
      .catch((error) => {
        console.error(error);
        setIsFetching(false);
      });
    setIsFetching(true);
  };

  // HANDLE CREATING DOG
  const handleCreateNewDog = () => {
    fetch(`${API_URL}/dogs`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ name: newTaskTitle, completed: false }),
    })
      .then((response) => {
        if (!response.ok) {
          setIsFetching(false);
          throw new Error("Failed to create dog");
        }
        return response.json();
      })
      .then((data) => {
        setIsFetching(false);
        setDogs((prevDogs) => [...prevDogs, data]);
        setNewTaskTitle("");
      })
      .catch((error) => {
        setIsFetching(false);
        console.error(error);
      });
    setIsFetching(true);
  };

  const handleDeleteDog = (id) => {
    fetch(`${API_URL}/dogs/${id}`, {
      method: "DELETE",
    })
      .then((response) => {
        if (!response.ok) {
          setIsFetching(false);
          throw new Error("Failed to delete dog");
        }
      })
      .then(() => {
        setDogs((prevDogs) => prevDogs.filter((dog) => dog.id !== id));
        setIsFetching(false);
      })
      .catch((error) => {
        console.error(error);
        setIsFetching(false);
      });
    setIsFetching(true);
  };

  const handleNewTaskChanged = (event) => {
    setNewTaskTitle(event.target.value);
  };

  return (
    <>
      <h1>Dog App</h1>
      <p
        style={{
          color: isFetching ? "red" : "green",
        }}
      >
        Fetch Status: {isFetching ? "fetching" : "not fetching"}
      </p>

      <h2>Create Dog</h2>
      <div>
        <input
          type="text"
          placeholder="New Dog"
          onChange={handleNewTaskChanged}
          value={newTaskTitle}
        />
        <button onClick={handleCreateNewDog} disabled={isFetching}>
          Create
        </button>
      </div>
      <h2>Dogs</h2>
      <ul>
        {dogs.map((dog) => (
          <li
            key={dog.id}
            style={{
              textDecoration: dog.completed ? "line-through" : "",
            }}
          >
            <input
              type="checkbox"
              checked={dog.completed}
              disabled={isFetching}
              onChange={(event) =>
                handleCheckedChangeRequest(dog.id, event.target.checked)
              }
            />
            {dog.name} (id: {dog.id})
            <button
              onClick={() => handleDeleteDog(dog.id)}
              disabled={isFetching}
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
    </>
  );
}

export default App;
