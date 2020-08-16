import React from 'react';
import ReactDOM from 'react-dom';
import App from './app/layout/App';
import * as serviceWorker from './serviceWorker';
import { Router } from 'react-router-dom';
import { Provider } from 'react-redux';
import store from './app/stores/store';
import { createBrowserHistory } from 'history';
export const history = createBrowserHistory();
ReactDOM.render(
  <Provider store={store}>
    <Router history={history}>
      <App />
    </Router>
  </Provider>,
  document.getElementById('root')
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
