from flask import Flask, request, render_template_string
import requests
from bs4 import BeautifulSoup

app = Flask(__name__)

@app.route('/search', methods=['POST'])
def search():
    query = request.form['query']
    response = requests.get(f'https://duckduckgo.com/?q={query}')
    soup = BeautifulSoup(response.text, 'html.parser')

    # Example: extract the title of the search results
    titles = [tag.get_text() for tag in soup.find_all('a', {'class': 'result__a'})]
    
    return render_template_string("""
        <h1>Search Results</h1>
        {% for title in titles %}
            <p>{{ title }}</p>
        {% endfor %}
    """, titles=titles)

if __name__ == '__main__':
    app.run(debug=True)
