import { combineReducers } from 'redux';
import genericReducer from './genericReducer';

const rootReducer = combineReducers({
  entities: genericReducer, 

});

export default rootReducer;
