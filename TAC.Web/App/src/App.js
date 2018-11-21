import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import FetchVehicles from './components/FetchVehicles';

export default () => (
    <Layout>
        <Route exact path='/' component={FetchVehicles} />
        <Route path='/FetchVehicles' component={FetchVehicles} />
  </Layout>
);
