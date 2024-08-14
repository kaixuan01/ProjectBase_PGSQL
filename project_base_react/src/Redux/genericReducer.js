import { INIT_DATA, UPDATE_DATA, DELETE_DATA, SET_ERROR } from './actionsTypes';

const genericReducer = (state = {}, action) => {
  const { entity, data, id, updates, error } = action.payload || {};

  switch (action.type) {
    case INIT_DATA:
      return {
        ...state,
        [entity]: { data: data || [], error: null },
      };
    case UPDATE_DATA:
      if (!state[entity]) return state;
      return {
        ...state,
        [entity]: {
          ...state[entity],
          data: state[entity].data.map((item) =>
            item.id === id ? { ...item, ...updates } : item
          ),
        },
      };
    case DELETE_DATA:
      if (!state[entity]) return state;
      return {
        ...state,
        [entity]: {
          ...state[entity],
          data: state[entity].data.filter((item) => item.id !== id),
        },
      };
    case SET_ERROR:
      return {
        ...state,
        [entity]: { ...(state[entity] || {}), error },
      };
    default:
      return state;
  }
};

export default genericReducer;
