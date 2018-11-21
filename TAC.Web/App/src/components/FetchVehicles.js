import React, { Component } from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { Link } from 'react-router-dom';
import { Moment } from 'moment';

class FetchVehicles extends Component {
    constructor(props) {
        super(props);

        this.state = {
            vehicles: null,
            customers: null,
            vehicleStatuses: null,
            time: new Date().toLocaleString()
        }
    }

    componentDidMount() {
        this.intervalID = setInterval(
            () => this.fetchAll(),
            1000
        );
    }
    componentWillUnmount() {
        clearInterval(this.intervalID);
    }

    fetchAll() {

        fetch('/api/VehicleManager/GetVehicles')
            .then(res => res.json())
            .then(vehicles => this.setState({ vehicles }));

        this.setState({
            time: new Date().toLocaleString()
        });

        //fetch('/api/VehicleManager/GetCustomers')
        //    .then(res => res.json())
        //    .then(customers => this.setState({ customers }));

        //fetch('/api/VehicleManager/GetVehicleStatuses')
        //    .then(res => res.json())
        //    .then(vehicleStatuses => this.setState({ vehicleStatuses }));
    }

    render() {
        return (
            <div>
                <h3 className="alert alert-info">{this.state.time}</h3>
                <table className="table">
                    <thead>
                        <tr>
                            <th>VIN</th>
                            <th>Regr. No</th>
                            <th>Connected On</th>
                            <th>Last Connected On</th>
                            <th>Status</th>
                            <th>Customer</th>
                        </tr>
                    </thead>
                    <tbody>
                        {(this.state.vehicles || []).map(v =>
                            <tr key={v.id}>
                                <td>{v.vin}</td>
                                <td>{v.registrationNumber}</td>
                                <td>{v.connectedOnDateTimeString}</td>
                                <td>{v.lastConnectedOnDateTimeString}</td>
                                <td>{v.vehicleStatusName}</td>
                                <td>{v.customer.name}</td>
                            </tr>)}
                    </tbody>
                </table>
            </div>
        )
    }
}

export default connect()(FetchVehicles)