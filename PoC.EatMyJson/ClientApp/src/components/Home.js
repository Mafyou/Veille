import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;
    constructor(props) {
        super(props);
        this.state = { myObject: [], loading: true };
        fetch('api/MyObject/GetMyObject')
            .then(response => response.json())
            .then(data => {
                this.setState({ myObject: data, loading: false });
            });
    }
    static renderLayout(myObject) {
        return (
            <div>
                <p>{myObject.title}</p>
                <p>{myObject.description}</p>
            </div>
        );
    }
  render () {
      let content = this.state.loading
          ? <p><img alt="loading" src={require('./img/loading.gif')} /> <em>Loading...</em></p>
          : Home.renderLayout(this.state.myObject);
      return (
          <section id="content">
              {content}
          </section>
          )
  }
}
