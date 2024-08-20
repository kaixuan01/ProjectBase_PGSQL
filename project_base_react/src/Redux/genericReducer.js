import { INIT_DATA, UPDATE_DATA, DELETE_DATA, SET_ERROR } from './actionsTypes';

const genericReducer = (state = {}, action) => {
  const { entity, data, id, updates, error } = action.payload || {};
console.log(data)
  switch (action.type) {
    case INIT_DATA:
      return {
        ...state,
        [entity]: data,
      };

      case UPDATE_DATA:
        return {
          ...state,
          [entity]: 
            data
          ,
        };

    case DELETE_DATA:
      return {
        ...state,
        [entity]: (state[entity] || []).filter(item => item.id !== id),
      };

    case SET_ERROR:
      return {
        ...state,
        errors: {
          ...state.errors,
          [entity]: error,
        },
      };

    default:
      return state;
  }
};

export default genericReducer;
