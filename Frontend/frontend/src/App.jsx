import { useEffect, useState } from 'react';
import { API_URL } from './consts';
import './App.css'

function App() {
  const [cats, setCats] = useState([]);
  const [isFetching, setIsFetching] = useState(true);
  const [newCatName, setNewCatName] = useState("");
  const [newCatAge, setNewCatAge] = useState("");

  // Load existing cats
  useEffect(() => {
    fetch(`${API_URL}/cats`)
      .then((response) => response.json())
      .then((data) => {
        setCats(data);
        setIsFetching(false);
      });
  }, []);

  // Handle updating cats
  const handleRatingChange = (id, number) => {
    // Find the cat's current rating
    const catRating = cats.find(cat => cat.id === id).rating;
    fetch(`${API_URL}/cats/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ rating: (catRating + number)})
    })
      .then((response) => {
        if (!response.ok) {
          setIsFetching(false);
          throw new Error("Failed to update cats");
        }
      })
      .then(() => {
        setCats((prevCats) => 
          prevCats.map((cat) => {
            if (cat.id === id) {
              return { ...cat, rating: (catRating + number)};
            }
            return cat;
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

  // Handle creating cats
  const handleCreateNewCat = () => {
    const catAge = Number(newCatAge);
    const catAgeToSet = isNaN(catAge) ? 0 : catAge;
    fetch(`${API_URL}/cats`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ name: newCatName, age: catAgeToSet }),
    })
      .then((response) => {
        if (!response.ok) {
          setIsFetching(false);
          throw new Error("Failed to create cat");
        }
        return response.json();
      })
      .then((data) => {
        setIsFetching(false);
        setCats((prevCats) => [...prevCats, data]);
        setNewCatName("");
        setNewCatAge("");
      })
      .catch((error) => {
        setIsFetching(false);
        console.error(error);
      });
    setIsFetching(true);
  };

  const handleDeleteCat = (id) => {
    fetch(`${API_URL}/cats/${id}`, {
      method: "DELETE",
    })
      .then((response) => {
        if (!response.ok) {
          setIsFetching(false);
          throw new Error("Failed to delete cat");
        }
      })
      .then(() => {
        setCats((prevCats) => prevCats.filter((cat) => cat.id !== id));
        setIsFetching(false);
      })
      .catch((error) => {
        console.error(error);
        setIsFetching(false);
      });
    setIsFetching(true);
  };

  const handleNewCatNameChanged = (event) => {
    setNewCatName(event.target.value);
  };

  const handleNewCatAgeChanged = (event) => {
    setNewCatAge(event.target.value);
  }

  return (
    <>
      <h1>Cat App</h1>
      <p
        style={{
          color: isFetching ? "red" : "green",
        }}
      >
        Fetch Status: {isFetching ? "fetching" : "not fetching"}
      </p>

      <h2>Create Cat</h2>
      <div>
        <input
          type="text"
          placeholder="Cat Name"
          onChange={handleNewCatNameChanged}
          value={newCatName}
        />
        <input
          type="text"
          placeholder="Cat Age (e.g 0)"
          onChange={handleNewCatAgeChanged}
          value={newCatAge}
        />
        <button onClick={handleCreateNewCat} disabled={isFetching}>
          Create
        </button>
      </div>
      <h2>Cats</h2>
      <ul>
        {cats.map((cat) => (
          <li
            key={cat.id}
          >
            {cat.name} | Age: {cat.age} |
            <button
              disabled={isFetching}
              onClick={() => handleRatingChange(cat.id, 1)}>
              Up
            </button>
            <h3>Rating: {cat.rating}</h3>
            <button
              disabled={isFetching}
              onClick={() => handleRatingChange(cat.id, -1)}>
              Down
            </button>
            <button
              onClick={() => handleDeleteCat(cat.id)}
              disabled={isFetching}
            >
              Delete
            </button>
          </li>
        ))}
      </ul>
    </>
  )
}

export default App
