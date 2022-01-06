import '../styles/main-styles.css';
import '../styles/data-styles.css';

export const InputData = () => {
    return (
      <div className="data__input-data">
        <form className="data__input-data--form">
          <input
            type='text'
            id="text"
            name="text"
            placeholder="Wyszukaj"
          />
          <select id="search" className="custom-select">
            <option value="1">Opcja 1</option>
            <option value="2">Opcja 2</option>
            <option value="3">Opcja 3</option>
            <option value="4">Opcja 4</option>
          </select>
        </form>
      </div>
      );
}